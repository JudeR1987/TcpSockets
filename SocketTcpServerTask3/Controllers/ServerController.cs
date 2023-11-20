namespace SocketTcpServerTask3.Controllers;

// Серверные операции
public class ServerController
{
    // 1. date – возвращает дату и время на сервере
    public string Date() => $"{DateTime.Now:dd.MM.yyyyг. - HH:mm:ss}";


    // 2. host_name возвращает имя компьютера, на котором работает сервер
    public string HostName() => $"{Environment.MachineName}";


    // 3. pwd – возвращает полное имя папки App_Files приложения
    public string Pwd() {

        var path = $"{Environment.CurrentDirectory}\\App_Files";

        return Directory.Exists(path)
            ? path
            : "not found";

    } // Pwd


    // 4. list – клиент получает список имен файлов, хранящихся
    // на сервере, в папке App_Files (в папке исполняемого файла),
    // имена файлов разделены строкой “\n”
    public string List() {

        var path = $"{Environment.CurrentDirectory}\\App_Files";

        var i = 1;
        var answer = Directory.Exists(path)
            ? "\n" + string.Join("\n", Directory.GetFiles(path)
                    .Select(f => $"{i++}. " + Path.GetFileName(f)!)
                    .ToList())
            : "not found";

        return answer;

    } // List


    // 5. mul число1 число2 – сервер возвращает строку, содержащую
    // два вещественных числа и произведение этих чисел
    public string Mul (string[] tokens) {

        string answer;

        // получить числа из строки запроса
        if (tokens.Length != 3) {

            answer = "mul: invalid arguments number";

        } else {

            var number1 = double.Parse(tokens[1]);
            var number2 = double.Parse(tokens[2]);
            answer = $"{number1:n3} * {number2:n3} = {number1 * number2:n3}";

        } // if

        return answer;

    } // Mul


    // 6. sum число1 число2 – сервер возвращает строку, содержащую
    // два вещественных числа и сумму этих чисел
    public string Sum (string[] tokens) {

        string answer;

        // получить числа из строки запроса
        if (tokens.Length != 3) {

            answer = "sum: invalid arguments number";

        } else {

            var number1 = double.Parse(tokens[1]);
            var number2 = double.Parse(tokens[2]);
            answer = $"{number1:n3} + {number2:n3} = {number1 + number2:n3}";

        } // if

        return answer;

    } // Sum


    // 7. solve a b c – сервер возвращает три числа a, b, c
    // и вычисленные корни квадратного уравнения 〖a∙x〗^2+b∙x+c=0,
    // при отсутствии действительных корней возвращать числа a, b,
    // c и строку “\nнет корней\n”
    public string Solve(string[] tokens) {

        string answer;

        if (tokens.Length != 4) {

            answer = "solve: invalid arguments number";

        } else {

            var a = double.Parse(tokens[1]);
            var b = double.Parse(tokens[2]);
            var c = double.Parse(tokens[3]);
            answer = $"A = {a:n3}, B = {b:n3}, C = {c:n3}: ";

            var d = b * b - 4 * a * c;
            if (!a.Equals(0) && d >= 0) {

                var t = 2 * a;
                d = Math.Sqrt(d);
                var x1 = (-b - d) / t;
                var x2 = (-b + d) / t;
                answer += $"X1 = {x1:n3}, X2 = {x2:n3}.";

            } else {

                answer += "\nнет корней\n";

            } // if

        } // if

        return answer;

    } // Solve


    // 8. div число1 число2 – сервер возвращает строку, содержащую
    // два вещественных числа и частное этих чисел, если число2 == 0,
    // возвращать строку “\ndivide by zero\n
    public string Div(string[] tokens) {

        string answer;

        if (tokens.Length != 3) {

            answer = "div: invalid arguments number";

        } else {

            // получить числа из строки запроса
            var number1 = double.Parse(tokens[1]);
            var number2 = double.Parse(tokens[2]);

            // при делении на 0 - вернуть соответствующее сообщение
            if (number2.Equals(0d)) {

                answer = "\ndivide by zero\n";

            } else {

                answer = $"{number1:n3} / {number2:n3} = {number1 / number2:n3}";

            } // if

        } // if

        return answer;

    } // Div


    // 10. rename староеИмя новоеИмя – переименование файла в папке
    // App_Files на сервере (в папке исполняемого файла), сервер
    // возвращает “Ok\n”, если файл был переименован на сервере 
    // или строку "Not found\n" – если такого файла на сервере нет
    public string Rename(string[] tokens) {

        string answer;

        if (tokens.Length != 3) {

            answer = "rename: invalid arguments number";

        } else {

            var path = $"{Environment.CurrentDirectory}\\App_Files";
            var srcFileName = path + "\\" + tokens[1];
            var dstFileName = path + "\\" + tokens[2];

            // попытка переименования, при успехе возвращаем клиенту "Ok",
            // при любой ошибке возвращаем клиенту "Not found"
            try {

                File.Move(srcFileName, dstFileName);
                answer = "Ok\n";

            } catch {

                answer = "Not found\n";

            } // try-catch

        } // if

        return answer;

    } // Rename


    // 11. upload имяФайла – клиент выбирает файл и отправляет его
    // на сервер, сначала строка “Length ДлинаВБайтах\n”, затем
    // байты файла, далее строка “Ok\n”, сервер сохраняет принятый
    // файл в папку App_Files (в папке исполняемого файла)
    public string Upload(string[] tokens) {

        string answer = string.Empty;

        /*// Из коллекции байт получить имя файла
        var temp = Encoding.UTF8.GetString(bytes.TakeWhile(b => b != 10).ToArray());
        Console.WriteLine(temp);
        var tokens = temp.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        Array.ForEach(tokens, Console.WriteLine);

        // Из коллекции байт получить длину файла в байтах
        if (tokens.Length != 2) {

            answer = "upload: invalid arguments number";

        } else {

            var path = $"{Environment.CurrentDirectory}\\App_Files";
            var fileName = path + "\\" + tokens[1];

            // попытка загрузки файла на сервер
            // при успехе возвращаем клиенту "Ok"
            // при любой ошибке возвращаем клиенту "Fail"
            try {

                // Записать файл по заданному имени
                // File.WriteAllBytes(fileName, bytes.Skip(header).Take(length).ToArray());
                answer = "Ok\n";

            } catch {

                answer = "Fail\n";

            } // try-catch

        } // if*/

        return answer;

    } // Upload


    // 12. download имяФайла – сервер передает запрошенный файл
    // из папки App_Files клиенту: строка "Ok ДлинаВБайтах\n"
    // и затем передача байтов файла, далее строка “Ok\n”, если
    // такой файл есть не сервере или строку "Not found\n", если
    // такого файла на сервере нет
    public string Download(string[] tokens) {

        string answer;

        if (tokens.Length != 2) {

            answer = "download: invalid arguments number";

        } else {

            var path = $"{Environment.CurrentDirectory}\\App_Files";
            var fileName = path + "\\" + tokens[1];

            // попытка выгрузки файла с сервера
            // при успехе возвращаем клиенту "Ok"
            // при любой ошибке возвращаем клиенту "Fail"
            try {

                answer = "Ok\n";

            } catch {

                answer = "Fail\n";

            } // try-catch

        } // if

        return answer;

    } // Download


    // 13. delete имяФайла – удаляет файл на сервере, в папке App_Files
    // проекта (в папке исполняемого файла), сервер возвращает
    // “Ok\n”, если файл был удален на сервере или строку
    // "Not found\n" – если такого файла на сервере нет
    public string Delete (string[] tokens) {

        string answer;

        if (tokens.Length != 2) {

            answer = "delete: invalid arguments number";

        } else {

            var path = $"{Environment.CurrentDirectory}\\App_Files";
            var fileName = path + "\\" + tokens[1];

            // попытка удаления, при успехе возвращаем клиенту "Ok",
            // при любой ошибке возвращаем клиенту "Not found"
            // Метод Delete() не выбрасывает исключений
            if (File.Exists(fileName)) {

                File.Delete(fileName);
                answer = "Ok\n";

            } else {

                answer = "Not found\b";

            } // if

        } // if

        return answer;

    } // Delete

} // class ServerController