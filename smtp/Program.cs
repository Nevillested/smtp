using System;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Threading.Tasks;
using System.Text;


namespace NetConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Если в https://myaccount.google.com/security включена двухфакторная аутентификация, то:" +
                "\r\n 1. Надо перейти сюда https://myaccount.google.com/apppasswords " +
                "\r\n 2. В поле 'select app' выбрать 'mail'" +
                "\r\n 3. В поле 'select device' выбрать 'windows computer'" +
                "\r\n 4. Нажать кнопочку 'generate'" +
                "\r\n 5. Сохранить сгенерированный пароль для следущего ввода");
            Console.WriteLine("Введите вашу gmail почту (ex: blahblah@mail.com):");
            string frommail = Console.ReadLine();

            Console.WriteLine("Имя отправителя:");
            string fromname = Console.ReadLine();

            Console.WriteLine("Адрес получателя (ex: blahblah@mail.com):");
            string tomail = Console.ReadLine();

            Console.WriteLine("Имя получателя");
            string toname = Console.ReadLine();

            var fromAddress = new MailAddress(frommail, fromname);
            var toAddress = new MailAddress(tomail, toname);
            
            Console.WriteLine("Если у вас включена двухфакторная аутентификация, введите сгенерированный раннее пароль, иначе-пароль от почты");
            string fromPassword = Console.ReadLine();

            Console.WriteLine("Введите тему письма");
            string subject = Console.ReadLine();
            Console.WriteLine("Введите тело письма");
            string body = Console.ReadLine();

            //Дополнительная задача
            //II. В качестве дополнительного параметра задается ключевое слово. 
            //По данному ключевому слову выполняется поиск в текстовых файлах в папке клиента.
            //При обнаружении слова файл прикрепляется к письму.
            //получаем названия всех файлов, заранее мной созданных
            DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory() + "/files_for_test");

            FileInfo[] Files = d.GetFiles("*.txt");

            //выводим названия в консоль, для наглядности

            Console.WriteLine("Текущая директория (заранее созданная) я файлами содержит файлы:");
            foreach (FileInfo file in Files)
            {
                Console.WriteLine(file.Name);
            }

            Console.WriteLine("По какому ключевому слову будем искать файл?");
            string key_word = Console.ReadLine();
            string path_our_file = "";
            foreach (FileInfo file in Files)
            {
                if (file.Name.ToLower().Contains(key_word.ToLower()))
                {
                    path_our_file = d + "/" + file.Name;
                }
            }
            Console.WriteLine(path_our_file);

            //создаем экземпляр вложения нашего файла
            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(path_our_file);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress) {Subject = subject, Body = body} )
            {

                message.Attachments.Add(attachment);
                smtp.Send(message);
            }
            Console.WriteLine("Отправлено! Нажмите любую кнопку.");
            Console.ReadLine();
        }
    }
}
