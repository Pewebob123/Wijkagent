﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using WijkAgent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WijkAgent.Model.Tests
{
    [TestClass()]
    public class TwitterTests
    {

        #region Twitter

        #region SearchResultsTest_ShouldFindNoResult_WhenRadiusIsZero
        [TestMethod()]
        public void SearchResultsTest_ShouldFindNoResult_WhenRadiusIsZero()
        {
            // arrange
            double latitude = 51.979745;
            double longitude = 5.901053;
            int radius = 0;
            int maxResults = 1000;

            Twitter twitter = new Twitter();

            // act
            twitter.SearchResults(latitude, longitude, radius, maxResults);

            // assert
            Assert.AreEqual(0, twitter.tweetsList.Count);
        }
        #endregion

        #region AddTweets_ShouldAddTweetToList_WhenTimeIsLessThan24HoursAgo
        [TestMethod()]
        public void AddTweets_ShouldAddTweetToList_WhenTimeIsLessThan24HoursAgo()
        {
            // arrange
            int id = 1;
            double latitude = 51.979745;
            double longitude = 5.901053;
            string user = "Ruben";
            string message = "Dit is een unit test";
            DateTime date = DateTime.Now.AddHours(-23);
            DateTime limitTime = DateTime.Now.AddHours(-24);
            Tweet tweet = new Tweet(id, latitude, longitude, user, message, date, limitTime);
            Twitter twitter = new Twitter();

            // act
            twitter.AddTweets(tweet);

            // assert
            Assert.AreEqual(1, twitter.tweetsList.Count);
        }
        #endregion

        #region AddTweets_ShouldNotAddTweetToList_WhenTimeIsMoreThan24HoursAgo
        [TestMethod()]
        public void AddTweets_ShouldNotAddTweetToList_WhenTimeIsMoreThan24HoursAgo()
        {
            // arrange
            int id = 1;
            double latitude = 51.979745;
            double longitude = 5.901053;
            string user = "Ruben";
            string message = "Dit is een unit test";
            DateTime date = DateTime.Now.AddHours(-26);
            DateTime limitTime = DateTime.Now.AddHours(-24);
            Tweet tweet = new Tweet(id, latitude, longitude, user, message, date, limitTime);
            Twitter twitter = new Twitter();

            // act
            twitter.AddTweets(tweet);

            // assert
            Assert.AreEqual(0, twitter.tweetsList.Count);
        }
        #endregion

        #endregion

        #region Tweet

        #region Tweet_UserShouldNotContainQuote_WhenTweetIsDeclared
        [TestMethod()]
        public void Tweet_UserShouldNotContainQuote_WhenTweetIsDeclared()
        {
            // arrange
            int id = 1;
            double latitude = 51.979745;
            double longitude = 5.901053;
            string user = "\"Ruben";
            string message = "Dit is een unit test";
            DateTime date = DateTime.Now.AddHours(-20);
            DateTime limitTime = DateTime.Now.AddHours(-24);
            Tweet tweet = new Tweet(id, latitude, longitude, user, message, date, limitTime);
            Twitter twitter = new Twitter();

            // act
            twitter.AddTweets(tweet);

            // assert
            Assert.AreEqual(false, twitter.tweetsList[0].user.Contains("\""));
        }
        #endregion

        #region Tweet_ShouldAddItemToLinkList_WhenMessageContainsHttp
        [TestMethod()]
        public void Tweet_ShouldAddItemToLinkList_WhenMessageContainsHttp()
        {
            // arrange
            int id = 1;
            double latitude = 51.979745;
            double longitude = 5.901053;
            string user = "Ruben";
            string message = "Dit is een unit test met een link. http://test.com";
            DateTime date = DateTime.Now.AddHours(-20);
            DateTime limitTime = DateTime.Now.AddHours(-24);
            Tweet tweet = new Tweet(id, latitude, longitude, user, message, date, limitTime);
            Twitter twitter = new Twitter();

            // act
            twitter.AddTweets(tweet);

            // assert
            Assert.AreEqual(1, tweet.links.Count);
        }
        #endregion
        #endregion

        #region Map
        [TestMethod()]
        public void calculateRadiusKm_ShouldReturnRadius_WhenCoordinatesAreGiven()
        {
            // arrange
            Map map = new Map();
            List<double> latitudePoints = new List<double>();
            List<double> longitudePoints = new List<double>();
            latitudePoints.Add(52.500385);
            latitudePoints.Add(52.503833);
            latitudePoints.Add(52.509658);
            latitudePoints.Add(52.507072);
            longitudePoints.Add(6.055248);
            longitudePoints.Add(6.047266);
            longitudePoints.Add(6.055119);
            longitudePoints.Add(6.066792);
            double centerLat = (latitudePoints.Max() + latitudePoints.Min()) / 2;
            double centerLong = (longitudePoints.Max() + longitudePoints.Min()) / 2;

            double expectedOutcome = 0.69961280774376;

            // act
            double radius = map.calculateRadiusKm(latitudePoints, longitudePoints, centerLat, centerLong);
            string stringRadius = string.Format("{0:0.00}", radius.ToString());
            radius = Convert.ToDouble(stringRadius);

            // assert
            Assert.AreEqual(expectedOutcome, radius);
        }
        #endregion

        #region LoginScreen
        [TestMethod()]
        public void getSHA512_ShouldEncryptPassword_WhenMethodIsCalled()
        {
            // arrange
            LogInScreen loginscreen = new LogInScreen();
            string password = "password";
            string expected = "b109f3bbbc244eb82441917ed06d618b9008dd09b3befd1b5e07394c706a8bb980b1d7785e5976ec049b46df5f1326af5a2ea6d103fd07c95385ffab0cacbc86";

            // act
            string encryptedPassword = loginscreen.getSHA512(password);
            
            // assert
            Assert.AreEqual(expected, encryptedPassword);
        }
        #endregion
    }
}