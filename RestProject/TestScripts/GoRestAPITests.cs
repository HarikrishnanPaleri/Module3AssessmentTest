using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json;
using RestProject.HelperClasses;
using RestProject.Utilities;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestProject.TestScripts
{
    [TestFixture]
    internal class GoRestAPITests : CoreCodes
    {
        [Test]
        [Category("Get")]
        [Order(1)]
        public void GetAllUserDetails()
        {

            test = extent.CreateTest("Get All User");
            Log.Information("Get all user test");
            var request = new RestRequest("", Method.Get);
            var response = client.Execute(request);

            try
            {
                Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK)); //Nunit assertion
                Log.Information($"API Response: {response.Content}");

                List<UserData> users = JsonConvert.DeserializeObject<List<UserData>>(response.Content);

                Assert.NotNull(users);
                Log.Information("Get All User details test passed"); //Log File Created

                test.Pass("Get All User details test passed"); //ExtentReport created
            }
            catch (AssertionException)
            {
                test.Fail("Get All User Details test failed");
            }
        }
        [Test]
        [TestCase(5838994)] //parameterisation
        [Category("Get")]
        [Order(2)]
        public void GetSingleUserDetails(int id)
        {

            test = extent.CreateTest("Get single User");
            Log.Information("Get single user test");
            var request = new RestRequest("" + id, Method.Get);
            var response = client.Execute(request);

            try
            {
                Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
                Log.Information($"API Response: {response.Content}");
                var users = JsonConvert.DeserializeObject<UserData>(response.Content);

                Assert.NotNull(users?.Email);
                Log.Information($"User Email matches with fetch {users.Email}");
                Log.Information("Get single User details test passed");
                test.Pass("Get single User details test passed");
            }
            catch (AssertionException)
            {
                test.Fail("Get single User Details test failed");
            }
        }
        [Test]
        [Category("POST")]
        [Order(0)]
        public void CreateUserTest()
        {
            test = extent.CreateTest("Create User");
            Log.Information("Create User Test Started");

            string bearerToken = "ecba7899ccddc784ac54426998d64d14fc643192f2e24fb8eff49632c6fd4034";
            var request = new RestRequest("", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {bearerToken}");
            request.AddJsonBody(new
            {
                name = "Jay",
                email = "Jay@ohl.com",
                gender = "male",
                status = "active"
            });
            var response = client.Execute(request);

            try
            {
                Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created));
                Log.Information($"API Response: {response.Content}");

                var user = JsonConvert.DeserializeObject<UserData>(response.Content);
                Assert.NotNull(user);
                Log.Information("User created and returned");
                Assert.That(user.Name, Is.EqualTo("Jay"));
                Log.Information($"User Name matches with fetch {user.Name}");
                Assert.That(user.Email, Is.EqualTo("Jay@ohl.com"));
                Log.Information($"User Email matches with fetch {user.Email}");
                Assert.That(user.Gender, Is.EqualTo("male"));
                Log.Information($"User Gender matches with fetch {user.Gender}");
                Assert.That(user.Status, Is.EqualTo("active"));
                Log.Information($"User Status matches with fetch {user.Status}");

                Log.Information("Create User test passed");

                test.Pass("Create User Test passed");
            }
            catch (AssertionException)
            {
                test.Fail("Create User test failed");
            }
        }
        [Test]
        [TestCase(1830526)] //parameteisation
        [Category("PUT")]
        [Order(3)]
        public void UpdateUser(int Uid)
        {
            test = extent.CreateTest("Update User Test");
            Log.Information("Update User Test");
            string bearerToken = "ecba7899ccddc784ac54426998d64d14fc643192f2e24fb8eff49632c6fd4034";
            var request = new RestRequest("" + Uid, Method.Put);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {bearerToken}");
            request.AddJsonBody(new
            {
                name = "Doni",
                email = "DoniCarlos@gmail.com",
                gender = "male",
                status = "active"
            });
            var response = client.Execute(request);
            try
            {
                Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
                Log.Information($"Api Response:{response.Content}");
                var Userdetails = JsonConvert.DeserializeObject<UserData>(response.Content);
                Assert.NotNull(Userdetails);
                Log.Information("User Details Updated");
                Assert.That(Userdetails.Name, Is.EqualTo("Doni"));
                Log.Information("Update User Test Passed");
                Assert.That(Userdetails.Email, Is.EqualTo("DoniCarlos@gmail.com"));
                Log.Information("Update User Test Passed");
                test.Pass("Update User Test Passed");


            }
            catch (AssertionException)
            {
                test.Fail("Update User Test Failed");
            }
        }

        [Test]
        [TestCase(5838983)]  //parameterisation
        [Category("Delete")]
        [Order(4)]
        public void DeleteUser(int uid)
        {
            test = extent.CreateTest("Delete User Test");
            Log.Information("Delete User Test");
            string bearerToken = "ecba7899ccddc784ac54426998d64d14fc643192f2e24fb8eff49632c6fd4034";
            var request = new RestRequest(""+uid, Method.Delete);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {bearerToken}");

            var response = client.Execute(request);
            try
            {
                Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NoContent));
                Log.Information($"Api Error:{response.Content}");
                var data = JsonConvert.DeserializeObject<UserData>(response.Content);
                Assert.IsNull(data);
                Log.Information("Delete Test Passed");
                test.Pass("Delete User Test Passed");

            }
            catch (AssertionException)
            {
                test.Fail("Delete user Test");
            }
        }
    }
}
