using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Test.Constants;
using Test.interfaces;
using Test.Models;

namespace Test.Service
{
    class CardRepository : ICardRepository
    {

        public async Task AddCardAsync(Book book)
        {
            if (!Directory.Exists(PathConstants.PathToFolder))
            {
                Directory.CreateDirectory(PathConstants.PathToFolder);
            }

            string id = Guid.NewGuid().ToString();

            var newBook = JsonConvert.SerializeObject(new Book
            {
                Id = id,
                Name = book.Name,
                Picture = book.Picture
            });

            await Task.Run(() =>
            {
                string filePath = PathConstants.PathToFolder + id + ".json";
                File.WriteAllText(filePath, newBook);
            });

        }

        public async Task<bool> DeleteCardAsync(string id)
        {
            string pathFile = PathConstants.PathToFolder + id + ".json";
            if (File.Exists(pathFile))
            {
                try
                {
                    await Task.Run(() =>
                    {
                        File.Delete(pathFile);
                    });
                }
                catch
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public async Task<bool> EditCardAsync(string id, Book book)
        {

            if (Directory.Exists(PathConstants.PathToFolder))
            {
                string[] fileEntries = Directory.GetFiles(PathConstants.PathToFolder);
                if (fileEntries != null)
                {
                    try
                    {
                        await Task.Run(() =>
                        {
                            foreach (string fileName in fileEntries)
                            {
                                string name = Path.GetFileName(fileName);

                                if (name == id + ".json")
                                {
                                    StreamReader fileRead = new StreamReader(fileName);
                                    string jsonBook = fileRead.ReadToEnd();
                                    fileRead.Dispose();
                                    var editBook = JsonConvert.DeserializeObject<Book>(jsonBook);

                                    var newBook = JsonConvert.SerializeObject(new Book
                                    {
                                        Id = editBook.Id,
                                        Name = !string.IsNullOrEmpty(book.Name) ? book.Name : editBook.Name,
                                        Picture = (book.Picture) != null ? book.Picture : editBook.Picture
                                    });
                                    File.WriteAllText(fileName, newBook);
                                }
                            }
                        });
                    }
                    catch
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        public async Task<List<Book>> GetCardsAsync()
        {
            List<Book> cards = new List<Book>();

            if (Directory.Exists(PathConstants.PathToFolder))
            {

                await Task.Run(() =>
                {
                    string[] fileEntries = Directory.GetFiles(PathConstants.PathToFolder);
                    if (fileEntries != null)
                    {
                        foreach (string fileName in fileEntries)
                        {
                            string name = Path.GetFileName(fileName);

                            StreamReader fileRead = new StreamReader(fileName);
                            string json = fileRead.ReadToEnd();
                            fileRead.Dispose();

                            var newBook = JsonConvert.DeserializeObject<Book>(json);
                            cards.Add(newBook);
                        }
                    }
                });

                return cards;
            }
            else
            {
                return cards;
            }
        }

        public async Task<Book> GetCardByIdAsync(string id)
        {
            Book book = null;
            if (Directory.Exists(PathConstants.PathToFolder))
            {
                string[] fileEntries = Directory.GetFiles(PathConstants.PathToFolder);

                if (fileEntries != null)
                {
                    await Task.Run(() =>
                    {
                        foreach (string fileName in fileEntries)
                        {
                            string name = Path.GetFileName(fileName);
                            if (name == id + ".json")
                            {
                                StreamReader fileRead = new StreamReader(fileName);
                                string json = fileRead.ReadToEnd();
                                fileRead.Dispose();

                                book = JsonConvert.DeserializeObject<Book>(json);
                            }
                        }
                    });
                }
            }
            return book;
        }
    }
}
