﻿using System.Linq.Expressions;

using FluentValidation;

namespace UrlShortener.Api.Extensions;

internal static class AbstractValidatorAuthExtensions
{
    public const string AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";

    public const string AllowedPasswordCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+?.,";

    public static void RuleForUsername<T>(this AbstractValidator<T> validator, Expression<Func<T, string>> expression)
    {
        validator.RuleFor(expression)
            .NotEmpty()
            .Length(5, 20)
            .Must(x => x == null || x.All(c => AllowedUserNameCharacters.Contains(c)))
            .WithMessage($"Username must only contain the following characters: {AllowedUserNameCharacters}");
    }

    public static void RuleForPassword<T>(this AbstractValidator<T> validator, Expression<Func<T, string>> expression)
    {
        validator.RuleFor(expression)
            .NotEmpty()
            .Length(8, 100)
            .Must(x => x == null || x.All(c => AllowedPasswordCharacters.Contains(c)))
            .WithMessage($"Password must only contain the following characters: {AllowedPasswordCharacters}");
    }
}