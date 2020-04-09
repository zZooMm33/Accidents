using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accidents.Utils
{
    public static class SeparationTitleAndDescription
    {
        public static string[] Separation(string titleAndDescription)
        {
            int numbSeparation = 0, titleAndDescriptionLength = titleAndDescription.Length;
            string[] res = new string[2];

            for (int i = 1; i < titleAndDescriptionLength; i++)
            {
                if (Char.IsUpper(titleAndDescription[i]))
                {
                    numbSeparation = i;
                    break;
                }                             
            }

            res[0] = titleAndDescription.Substring(0, numbSeparation - 1);
            res[1] = titleAndDescription.Substring(numbSeparation, titleAndDescriptionLength - numbSeparation - 1);

            // убираем лишние пробелы из названия
            for (int i = res[0].Length - 1; i > 0; i--)
            {
                if (res[0][i] != ' ' && res[0][i] != '\r' && res[0][i] != '\n')
                {
                    numbSeparation = i + 1;
                    break;
                }
            }

            res[0] = res[0].Substring(0, numbSeparation);

            return res;
        }
    }
}