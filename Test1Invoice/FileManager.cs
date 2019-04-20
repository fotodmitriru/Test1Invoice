using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Test1Invoice
{
    /// <summary>
    /// Перечисление, которое содержит возможные состояния при работе с файлом.
    /// </summary>
    public enum StatusFile
    {
        FileNotFound,
        Ok,
        DirectoryNotFound,
        Failed,
        ErrorIO
    }
    /// <summary>
    /// Структура, содержащая список возможных сообщений при работе с файлом.
    /// </summary>
    public struct StatusFileMsg
    {
        /// <summary>
        /// Массив сообщений.
        /// </summary>
        public static string[] Msg { get; } =
        {
            "Файл не найден!", "OK", "Указанная директория не существует!",
            "Ошибка!", "Ошибка входных данных!"
        };
    }

    internal class FileManager
    {
        /// <summary>
        /// Метод сохраняет массив объектов в файл. Возвращает StatusFile.
        /// </summary>
        /// <param name="fileName">полное имя файла</param>
        /// <param name="arrayOfObjects">массив объектов, которые будут сохранены в БД</param>
        /// <returns>Возвращает StatusFile</returns>
        public StatusFile SaveToFile(string fileName, Object[] arrayOfObjects)
        {
            if (arrayOfObjects == null)
                return StatusFile.ErrorIO;
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, arrayOfObjects);
                    return StatusFile.Ok;
                }
            }
            catch (Exception ex)
            {
                if (ex is DirectoryNotFoundException)
                    return StatusFile.DirectoryNotFound;
            }

            return StatusFile.Failed;
        }
        /// <summary>
        /// Метод загружает данные из файла.
        /// </summary>
        /// <param name="fileName">полное имя файла</param>
        /// <param name="arrayOfObjects">массив типа Object[], в который будут записаны объекты, загруженные из файла</param>
        /// <returns>Возвращает StatusFile</returns>
        public StatusFile LoadFromFile(string fileName, out Object[] arrayOfObjects)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    arrayOfObjects = (Object[]) formatter.Deserialize(fs);
                    return StatusFile.Ok;
                }
            }
            catch (Exception ex)
            {
                arrayOfObjects = null;
                if (ex is FileNotFoundException)
                    return StatusFile.FileNotFound;
            }

            return StatusFile.Failed;
        }
    }
}
