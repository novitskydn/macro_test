using System;
using System.Net.Http;
using System.Xml;
using NUnit.Framework;

namespace MacroscopTests
{
    [TestFixture]
    public class MacroscopHttpApiTests
    {
        [Test]
        public void CheckTimeXML()
        {
            string urlToCheck = "http://demo.macroscop.com:8080/command?type=gettime&login=root&password=";

            //Create http client and get raw data from URL
            HttpClient httpClient = new HttpClient();
            string urlContent = httpClient.GetStringAsync(urlToCheck).Result;

            //Delete from received data everything except XML code
            int index1 = urlContent.IndexOf('<');
            string xmlContent = urlContent.Substring(index1);

            //Creating XML object with received data to parse date and time
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNodeList list = doc.GetElementsByTagName("string");
            string DateToConvert = (list[0].InnerXml);

            //Convert date and time from string to DateTime object
            DateTime DateToCompare = Convert.ToDateTime(DateToConvert);

            //Get local date and time and set to DateTime object
            DateTime localDate = DateTime.Now;

            //Checking equality of two DateTime objects within 15 seconds range
            Assert.That(localDate, Is.EqualTo(DateToCompare).Within(15).Seconds);
        }

        [Test]
        public void CheckTimeJSON()
        {
            string urlToCheck = "http://demo.macroscop.com:8080/command?type=gettime&login=root&password=&responsetype=json";

            //Create http client and get raw data from URL
            HttpClient httpClient = new HttpClient();
            string urlContent = httpClient.GetStringAsync(urlToCheck).Result;

            //Delete from received data everything except "JSON" code
            int index1 = urlContent.IndexOf('"');
            string cutContent = urlContent.Substring(index1);
            string DateToConvert = String.Join("", cutContent.Split('"'));

            //Convert date and time from string to DateTime object
            DateTime DateToCompare = Convert.ToDateTime(DateToConvert);

            //Get local date and time and set to DateTime object
            DateTime localDate = DateTime.Now;

            //Checking equality of two DateTime objects within 15 seconds range
            Assert.That(localDate, Is.EqualTo(DateToCompare).Within(15).Seconds);
        }

        [Test]
        public void CheckingNumberOfChannels()
        {
            string urlToCheck = "http://demo.macroscop.com:8080/configex?login=root&password=";

            //Create http client and get raw data from URL
            HttpClient httpClient = new HttpClient();
            string urlContent = httpClient.GetStringAsync(urlToCheck).Result;
            //Console.WriteLine(urlContent);

            //Creating XML object with received data to parse date and time
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(urlContent);
            XmlNodeList list = doc.GetElementsByTagName("ChannelInfo");
            
            //Get number of channels
            int NumberOfChannels = list.Count;
            
            Assert.That(NumberOfChannels, Is.GreaterThanOrEqualTo(6));
        }
    }
}