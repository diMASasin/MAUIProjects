using LovePdf.Core;
using LovePdf.Model.Enums;
using LovePdf.Model.Task;
using LovePdf.Model.TaskParams;
using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

public class Bot
{
    private static LovePdfApi _lovePdfApi = new LovePdfApi(
            "project_public_b66fa1a1079108856f36651ff5b03cf7_KZ37hc1b8950223815862fbb34f6fc579dbfd",
            "secret_key_b6263babaaa11fc963b6f41cc55fa494_wS6gS75a2439ccd6bf0e71206e57f983a0fd9");
    private static string _fileStoragePath = @"";

    public static void Main(string[] args)
    {
        var client = new TelegramBotClient("6217900739:AAGxehB5exvXseuZeJw7vfgGc4zI9TrBG9E");

        client.StartReceiving(Update, Error);
        Console.WriteLine("Start receiving");
        
        Console.ReadLine();
    }

    private async static Task Update(ITelegramBotClient telegramBot, Update update, CancellationToken arg3)
    {
        var message = update.Message;

        if (message.Text != null)
        {
            Console.WriteLine(message.Text);

            if (message.Text == "/start")
                await telegramBot.SendTextMessageAsync(message.Chat.Id, "Привет!");
        }

        if (message.Document != null)
        {
            var fileId = message.Document.FileId;
            SplitNameAndExtension(message.Document.FileName, out string fileName, out string fileExtension);

            if (fileExtension == "docx" || fileExtension == "doc")
            {
                await telegramBot.SendTextMessageAsync(message.Chat.Id, "Подождите, конвертирую файл(ы)");
                string destinationOfficeFilePath = _fileStoragePath + fileName + "." + fileExtension;
                await using Stream fileStream = System.IO.File.Create(destinationOfficeFilePath);
                var file = await telegramBot.GetInfoAndDownloadFileAsync(fileId, fileStream);
                fileStream.Close();

                await ConvertOfficeToPdf(destinationOfficeFilePath);
                System.IO.File.Delete(destinationOfficeFilePath);

                string destinationPdfFilePath = _fileStoragePath + fileName + ".pdf";
                await using Stream stream = System.IO.File.OpenRead(destinationPdfFilePath);
                await telegramBot.SendDocumentAsync(message.Chat.Id, InputFile.FromStream(stream, fileName + ".pdf"), caption: "Ваш pdf документ:");
                Console.WriteLine("Uploaded");
                stream.Close();
                System.IO.File.Delete(destinationPdfFilePath);
            }
            else
            {
                await telegramBot.SendTextMessageAsync(message.Chat.Id, $"Вы отправили {fileExtension} файл. Конвертация доступна только для файлов с расширением .doc/.docx");
                await SendFileSendingMessage(telegramBot, message.Chat.Id);
            }

        }
        else
        {
            await SendFileSendingMessage(telegramBot, message.Chat.Id);
        }
    }

    private static void SplitNameAndExtension(string fileNameAndExtension, out string fileName, out string fileExtension)
    {
        var fileNameAndExtensionList = fileNameAndExtension.Split('.').ToList();
        fileExtension = fileNameAndExtensionList.Last();
        fileNameAndExtensionList.Remove(fileExtension);
        fileName = string.Join(".", fileNameAndExtensionList);
        Console.WriteLine("fileName: " + fileName);
        Console.WriteLine("fileExtension: " + fileExtension);
    }

    private static Task Error(ITelegramBotClient arg1, Exception exception, CancellationToken arg3)
    {
        throw exception;
    }

    private static Task ConvertOfficeToPdf(string sourceFilePath)
    {
        var task = new Task(() =>
        {
            var pdfTask = _lovePdfApi.CreateTask<OfficeToPdfTask>();
            var file = pdfTask.AddFile(sourceFilePath);
            var time = pdfTask.Process();

            pdfTask.DownloadFile(_fileStoragePath);
            Console.WriteLine("Converted");
        });
        task.Start();
        return task;
    }

    private static Task SendFileSendingMessage(ITelegramBotClient telegramBot, long chatId)
    {
        return telegramBot.SendTextMessageAsync(chatId, "Опртправь файл с расширением .doc/.docx чтобы конвертировать его в pdf"); ;
    }
}