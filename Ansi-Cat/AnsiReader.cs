// <copyright file="AnsiReader.cs" company="Peter Ibbotson">
// Copyright (c) Peter Ibbotson. All rights reserved.
// </copyright>

namespace AnsiCat
{
    using System;
    using System.IO;

    /// <summary>
    /// Reads in an array of bytes.
    /// </summary>
    public class AnsiReader
    {
        /// <summary>
        /// Reads in the data, if the name starts with http uses the webclient
        /// to download from the web, otherwise reads from disk.
        /// </summary>
        /// <param name="name">Name of resource to read.</param>
        /// <returns>Array of bytes read.</returns>
        public static byte[] Read(string name)
        {
            if (name.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
            {
                using (var reader = new System.Net.WebClient())
                {
                    return reader.DownloadData(name);
                }
            }
            else
            {
                return File.ReadAllBytes(name);
            }
        }
    }
}
