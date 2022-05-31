using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Drift.Ui;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Drift
{
    [Serializable]
    public class GameConfiguration : IGameConfiguration, ISerializationCallbackReceiver
    {
        [SerializeField]
        private string currentLocaleCode;
        
        public IObservableValue<Locale> CurrentLocale { get; }
            = new ObservableValue<Locale>(null);
        
        public IEnumerable<Locale> AllLocales { get; private set; }

        public async UniTaskVoid Initialize()
        {
            await LocalizationSettings.InitializationOperation;
            
            AllLocales = LocalizationSettings.AvailableLocales.Locales;
            CurrentLocale.ValueChanged += CurrentLocaleOnValueChanged;

            SetCurrentLocaleFromCode();
            
            if (CurrentLocale.Value == null)
                CurrentLocale.Value = LocalizationSettings.SelectedLocale;
            else
                CurrentLocaleOnValueChanged(CurrentLocale.Value);
        }

        private void SetCurrentLocaleFromCode()
        {
            if (String.IsNullOrEmpty(currentLocaleCode))
            {
                CurrentLocale.Value = null;
                return;
            }

            try
            {
                CurrentLocale.Value = LocalizationSettings.AvailableLocales.GetLocale(
                    new LocaleIdentifier(currentLocaleCode));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                CurrentLocale.Value = LocalizationSettings.AvailableLocales.Locales.FirstOrDefault();
            }
        }

        private void CurrentLocaleOnValueChanged(Locale newLocale)
        {
            LocalizationSettings.SelectedLocale = newLocale;
        }

        public void OnBeforeSerialize()
        {
            if (CurrentLocale.Value == null)
            {
                currentLocaleCode = null;
                return;
            }

            currentLocaleCode = CurrentLocale.Value.Identifier.Code;
        }

        public void OnAfterDeserialize()
        {
        }
    }
}