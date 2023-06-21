using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace LA1305
{
    class Quiz
    {
        static void Main(string[] args)
        {
            // Verbindung zur MongoDB aufbauen
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("LA1305");
            var collection = database.GetCollection<BsonDocument>("questions");

            // Neue Frage hinzufügen oder Fragen beantworten
            string addQuestion;
            do
            {
                Console.WriteLine("Do you want to add a question? [y/n]");
                addQuestion = Console.ReadLine();

                if (addQuestion == "y")
                {
                    var instanceOfQuiz = new Quiz();
                    var newQuestion = instanceOfQuiz.AddQuestion();
                    collection.InsertOne(newQuestion);
                }

            } while (addQuestion == "y");

            // Liste aller Fragen initialisieren
            var allQuestions = GetAllQuestions(collection);

            // Immer 1 Frage auflisten und beantworten lassen
            while (allQuestions.Count > 0)
            {
                var randomIndex = new Random().Next(0, allQuestions.Count);
                var randomQuestion = allQuestions[randomIndex];

                Console.WriteLine("Question: " + randomQuestion["Question"]);
                Console.WriteLine("Enter your answer: ");
                string userAnswer = Console.ReadLine();

                // Antwort überprüfen
                string correctAnswer = randomQuestion["Answer"].AsString;
                if (userAnswer == correctAnswer)
                {
                    Console.WriteLine("Correct answer!");
                }
                else
                {
                    Console.WriteLine("Incorrect answer. The correct answer is: " + correctAnswer);
                }

                // Frage aus der Liste entfernen
                allQuestions.RemoveAt(randomIndex);
            }

            Console.WriteLine("No more questions available.");
        }

        public BsonDocument AddQuestion()
        {
            // Einfügen eines neuen Dokuments
            Console.WriteLine("Enter the new question: ");
            string newQuestion = Console.ReadLine();
            Console.WriteLine("Enter the answer: ");
            string newAnswer = Console.ReadLine();
            var document = new BsonDocument
            {
                { "Question", newQuestion},
                { "Answer", newAnswer}
            };
            return document;
        }

        public static List<BsonDocument> GetAllQuestions(IMongoCollection<BsonDocument> collection)
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            var questions = collection.Find(filter).ToList();
            return questions;
        }
    }
}
