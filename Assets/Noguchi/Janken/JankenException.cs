using System;
using UnityEngine;

public class JankenException : Exception
{
    public JankenException(string message) : base(message)
    {
        _ex = new Lazy<JankenException>(() => new JankenException($"{message}\n{message}"));
    }

    public JankenException Exception => _ex.Value;

    private readonly Lazy<JankenException> _ex;
}
