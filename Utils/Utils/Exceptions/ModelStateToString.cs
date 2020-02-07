using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CustomFramework.BaseWebApi.Resources;
using CustomFramework.BaseWebApi.Utils.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CustomFramework.BaseWebApi.Utils.Utils.Exceptions
{
    public static class ModelStateToStringConverter
    {
        public static string ModelStateToString(this ModelStateDictionary modelState, ILocalizationService localizationService)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException(DefaultResponseMessages.ModelStateValidError, nameof(modelState));
            }

            var modelStateStrings = modelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray();

            var newModelStateStrings = new List<string>();

            foreach (var modelStateString in modelStateStrings)
            {
                var errorString = modelStateString;
                var errorField = string.Empty;

                if (errorString.Contains("FieldLengthError"))
                {
                    var fieldName = GetField(errorString, localizationService);
                    var message = GetMessage(errorString, localizationService);
                    var constants = GetConstants(errorString, localizationService);

                    //TODO string değerlerinin ilk karakterini büyük harf yapan fonksiyonu CustomFramework.Utils içerisine yaz
                    var errorMessage = $"{char.ToUpper(fieldName.First()) + fieldName.Substring(1).ToLower()} {message} : {constants}";
                    newModelStateStrings.Add(errorMessage);
                }
                else if (errorString.Contains("RequiredError"))
                {
                    var fieldName = GetField(errorString, localizationService);
                    var message = GetMessage(errorString, localizationService);

                    var errorMessage = $"{char.ToUpper(fieldName.First()) + fieldName.Substring(1).ToLower()} {message}";
                    newModelStateStrings.Add(errorMessage);
                }
                else
                {

                    var localisedModelState = new StringBuilder();
                    var localisedModelStateArray = modelStateString.Split(':');
                    foreach (var item in localisedModelStateArray)
                    {
                        localisedModelState.Append($"{localizationService.GetValue(item.Trim())} -");
                    }
                    newModelStateStrings.Add(localisedModelState.ToString().Remove(localisedModelState.Length - 1, 1));
                }
            }

            return string.Join(" - ", newModelStateStrings.ToArray());
        }

        private static string GetFieldNameWithoutId(string fieldName, Match regexMatch, ILocalizationService localizationService)
        {
            //Id'li string değerleri için (CameraBrandId gibi) tercüme alan adı olacağı için Id stringi çıkarılıp o şekilde tercümeye gönderiliyor
            if (fieldName.ToString() == regexMatch.Groups[1].ToString() && fieldName.ToString().Contains("Id"))
            {
                return localizationService.GetValue(regexMatch.Groups[1].ToString().Remove(regexMatch.Groups[1].ToString().Length - 2));
            }
            return fieldName;
        }

        private static string GetField(string errorString, ILocalizationService localizationService)
        {
            var fieldNameRegex = new Regex("<field>(.*)</field>");
            var fieldNameRegexMatch = fieldNameRegex.Match(errorString);
            return GetFieldNameWithoutId(localizationService.GetValue(fieldNameRegexMatch.Groups[1].ToString()), fieldNameRegexMatch, localizationService);
        }

        private static string GetMessage(string errorString, ILocalizationService localizationService)
        {
            var messageRegex = new Regex("<message>(.*)</message>");
            var messageRegexMatch = messageRegex.Match(errorString);
            return localizationService.GetValue(messageRegexMatch.Groups[1].ToString());
        }

        private static string GetConstants(string errorString, ILocalizationService localizationService)
        {
            var constantRegex = new Regex("<const>(.*)</const>");
            var constantRegexMatch = constantRegex.Match(errorString);
            return constantRegexMatch.Groups[1].ToString();
        }
    }
}