﻿using System;
using WhiteMvvm.Behaviors.Base;
using Xamarin.Forms;

namespace WhiteMvvm.Behaviors
{
    public class ConfirmPasswordBehavior : ValidationBehavior
    {
        public static readonly BindableProperty ComparedTextProperty =
            BindableProperty.Create(nameof(OriginalPassword), typeof(string),
                typeof(ConfirmPasswordBehavior));
        
        protected override bool Validate(object value)
        {
            var confirmPasswordText = value?.ToString();
            return confirmPasswordText == OriginalPassword;
        }
        
        public string OriginalPassword
        {
            get => (string)GetValue(ComparedTextProperty);
            set => SetValue(ComparedTextProperty, value);
        }
    }}
