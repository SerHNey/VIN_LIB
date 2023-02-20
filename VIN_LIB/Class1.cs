using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VIN_LIB
{
    public class Class1
    {
        private Dictionary<string, string> countries = new Dictionary<string, string>();

        // Регулярка ниже парсит VIN и режет на отдельные куски.
        // wmi 
        // vds 
        // sign - Control sign
        // modelYear 
        // vis 
        public readonly Regex vinRule = new Regex(
            @"^(?<wmi>[a-z1-9-[oiq]]{3})(?<vds>[a-z0-9-[oiq]]{5})(?<sign>[0-9x]{1})(?<modelYear>[a-y1-9-[oiqu]]{1})(?<vis>[a-z0-9-[oiq]]{7})$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // Сгенерированные коды стран с их названиями.
        private readonly string countryCodes =
            "AA-AH ЮАРAJ-AN Котд'Ивуар;BA-BE Ангола;BF-BK Кения;BL-BR Танзания;CA-CE Бенин;CF-CK Мадагаскар;CL-CR Тунис;DA-DE " +
            "Египет;DF-DK Марокко;DL-DR Замбия;EA-EE Эфиопия;EF-EK Мозамбик;FA-FE Гана;FF-FK Нигерия;JA-JT Япония;KA-KE " +
            "ШриЛанка;KF-KK Израиль;KL-KR Южная Корея;KS-K0 Казахстан;LA-L0 Китай;MA-ME Индия;MF-MK Индонезия;ML-MR " +
            "Таиланд;NF-NK Пакистан;NL-NR Турция;PA-PE Филиппины;PF-PK Сингапур;PL-PR Малайзия;RA-RE ОАЭ;RF-RK Тайвань;RL-RR " +
            "Вьетнам;RS-R0 Саудовска я Аравия;SA-SM Великобритания;SN-ST Германия;SU-SZ Польша;S1-S4 Латвия;TA-TH " +
            "Швейцария;TJ-TP Чехия;TR-TV Венгрия;TW-T1 Португалия;UH-UM Дания;UN-UT Ирландия;UU-UZ Румыния;U5-U7 " +
            "Словакия;VA-VE Австрия;VF-VR Франция;VS-VW Испания;VX-V2 Сербия;V3-V5 Хорватия;V0-V6 Эстония;WA-W0 " +
            "Германия;XA-XE Болгария;XF-XK Греция;XL-XR Нидерланды;XS-XW СССР/СНГ;XX-X2 Люксембург;X0-X3 Россия;YA-YE " +
            "Бельгия;YF-YK Финляндия;YL-YR Мальта;YS-YW Швеция;YX-Y2 Норвегия;Y3-Y5 Беларусь;Y0-Y6 Украина;ZA-ZR Италия;" +
            "1A-10 США;2A-20 Канада;3A-3W Мексика;3X-37 Коста Рика;30-38 Каймановы острова;4A-40 США;5A-50 США;6A-6W " +
            "Австралия;7A-7E Новая Зеландия;8A-8E Аргентина;8F-8K Чили;8L-8R Эквадор;8S-8W Перу;8X-82 Венесуэла;9A-9E " +
            "Бразилия;9F-9K Колумбия;9L-9R Парагвай;9S-9W Уругвай;9X-92 Тринидад и Тобаго;93-99 Бразилия;ZX-Z2 " +
            "Словения;Z3-Z5 Литва;Z0-Z6 Россия";

        /// конструктор, заполняющий словарь yearsModel при инициализации.
        public Class1()
        {
            FillCountries();
        }
        /// Проверяет валидность VIN
        public bool CheckVIN(string vin)
        {
            return vinRule.Match(vin).Success;
        }

        /// Получает код-изготовителя транспорта
        public string GetVINCountry(string vin)
        {
            string countryCode = GetValue(vinRule.Match(vin), "wmi");
            foreach (KeyValuePair<string, string> entry in countries)
            {
                if (new Regex(entry.Key, RegexOptions.IgnoreCase | RegexOptions.Compiled).Match(countryCode).Success)
                    return entry.Key;
            }
            return "";
        }
        /// Разбирает значения, полученные с matched.
        public string GetValue(Match matched, string key)
        {
            if (matched.Success)
                return matched.Groups[key].Value;
            return "";
        }

        /// Заполняет словарь countries.
        private void FillCountries()
        {
            string[] codes = countryCodes.Split(';');
            foreach (string code in codes)
            {
                char[] sep = { ' ' };
                string[] codeInfo = code.Split(sep, 2);
                // Преобразует AA-AH в правило A[A-H]
                if ((codeInfo[0][1] >= 'A' && codeInfo[0][1] <= 'Z') && (codeInfo[0][4] >= '0' && codeInfo[0][4] <= '9'))
                    countries.Add(
                        codeInfo[0][0] + "[" + codeInfo[0][1] + "-Z0-" + codeInfo[0][4] + "]",
                        codeInfo[1]);
                else
                    countries.Add(
                        codeInfo[0][0] + "[" + codeInfo[0][1] + "-" + codeInfo[0][4] + "]",
                        codeInfo[1]);
            }
        }
        public bool CheckControlCount(string vin)
        {
            if (vin.Length != 17)
                return false;
            var result = 0;
            var index = 0;
            var checkDigit = 0;
            var checkSum = 0;
            var weight = 0;
            foreach (var c in vin.ToCharArray())
            {
                index++;
                var character = c.ToString().ToLower();
                if (index >= 1 && index <= 7 || index == 9)
                    weight = 9 - index;
                else if (index == 8)
                    weight = 10;
                else if (index >= 10 && index <= 17)
                    weight = 19 - index;
                if (index == 9)
                    checkDigit = character == "x" ? 10 : result;
                checkSum += (result * weight);
            }

            return checkSum % 11 == checkDigit;
        }
    }
}

