using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader
{
    internal class NameValidation
    {
        private char[] prohibitedCharacters = new char[10];

        public NameValidation()
        {
            prohibitedCharacters[0] = '<';
            prohibitedCharacters[1] = '>';
            prohibitedCharacters[2] = ':';
            prohibitedCharacters[3] = '\"';
            prohibitedCharacters[4] = '/';
            prohibitedCharacters[5] = '\\';
            prohibitedCharacters[6] = '|';
            prohibitedCharacters[7] = '?';
            prohibitedCharacters[8] = '*';
        }

        public bool ContainsProhibitedCharacters(string fileName)
        {
            bool response = false;
            foreach (char character in prohibitedCharacters)
            {
                if (fileName.Contains(character))
                {
                    response = true;
                }
            }
            return response;
        }
    }
}
