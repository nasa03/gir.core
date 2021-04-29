﻿namespace Repository.Model
{
    public enum Direction
    {
        Default,
        In,
        Out,
        Ref
    }

    public enum Transfer
    {
        Unknown,
        None,
        Container,
        Full
    }

    public enum Scope
    {
        Call,
        Async,
        Notified
    }
}