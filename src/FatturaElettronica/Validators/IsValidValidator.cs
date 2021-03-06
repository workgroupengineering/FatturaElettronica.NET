﻿using System;
#if NET35
using System.Linq;
#endif
using System.Collections.Generic;
using FatturaElettronica.Tabelle;
using FluentValidation.Validators;

namespace FatturaElettronica.Validators
{
    public class IsValidValidator<T> : PropertyValidator where T : Tabella, new()
    {
        private static readonly Lazy<T> DomainObjectLazy = new Lazy<T>(() => new T());

        protected override string GetDefaultMessageTemplate()
        {
            return "'{PropertyName}' valori accettati: {AcceptedValues}";
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
#if NET35
            context.MessageFormatter.AppendArgument("AcceptedValues", string.Format(string.Join(", ", Domain.ToArray())));
#else
            context.MessageFormatter.AppendArgument("AcceptedValues", string.Format(string.Join(", ", Domain)));
#endif
            if (context.PropertyValue is string codice)
                return Domain.Contains(codice);

            return false;
        }

        private static HashSet<string> Domain
        {
            get { return DomainObjectLazy.Value.Codici; }
        }
    }
}