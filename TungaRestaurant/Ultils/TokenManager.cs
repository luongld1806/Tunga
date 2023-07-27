using System.Security.Cryptography;
using System;
using TungaRestaurant.Models;
using System.Text;

namespace TungaRestaurant.Ultils
{
    public class TokenManager
    {
        private string GetToken(UserInfo user , string tokenId)
        {
            string hashItem = user.UserName + tokenId;
            SHA256 sha256 = SHA256.Create();
            sha256.ComputeHash(Encoding.UTF8.GetBytes(hashItem));
            return Convert.ToBase64String(sha256.Hash);
        }
        public string GetOrderToken(UserInfo user,string tokenId)
        {
            return GetToken(user,tokenId+"order");
        }
        public string GetReservationToken(UserInfo user, string tokenId)
        {
            return GetToken(user, tokenId + "reservation");
        }

    }
}
