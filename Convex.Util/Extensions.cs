﻿#region

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Convex.Event;
using Microsoft.Data.Sqlite;

#endregion

namespace Convex.Util
{
    public static class Extensions
    {
        /// <summary>
        ///     Obtain HTTP response from a GET request
        /// </summary>
        /// <returns>GET response</returns>
        public static async Task<string> HttpGet(this string instance)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(instance);
                string message = string.Empty;

                if (response.IsSuccessStatusCode)
                {
                    message = await response.Content.ReadAsStringAsync();
                }

                return message;
            }
        }

        /// <summary>
        ///     Splits a string into seperate parts
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="maxLength">max length of individual strings to split</param>
        public static IEnumerable<string> LengthSplit(this string instance, int maxLength)
        {
            for (int i = 0; i < instance.Length; i += maxLength)
            {
                yield return instance.Substring(i, Math.Min(maxLength, instance.Length - i));
            }
        }

        /// <summary>
        ///     Removes run-on spaces in a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DeliminateSpaces(this string str)
        {
            StringBuilder deliminatedSpaces = new StringBuilder();
            bool isSpace = false;

            // using for loop to increased cycle speed
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].Equals(' '))
                {
                    if (isSpace)
                    {
                        continue;
                    }

                    isSpace = true;
                }
                else
                {
                    isSpace = false;
                }

                deliminatedSpaces.Append(str[i]);
            }

            return deliminatedSpaces.ToString();
        }

        public static async Task QueryAsync(this SqliteConnection source, DatabaseQueriedEventArgs args)
        {
            await source.OpenAsync();

            using (SqliteTransaction transaction = source.BeginTransaction())
            {
                using (SqliteCommand command = source.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = args.Query;
                    await command.ExecuteNonQueryAsync();
                }

                transaction.Commit();
            }
        }

        public static void Query(this SqliteConnection source, DatabaseQueriedEventArgs args)
        {
            source.Open();

            using (SqliteTransaction transaction = source.BeginTransaction())
            {
                using (SqliteCommand command = source.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = args.Query;
                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
        }
    }
}
