﻿using System.Collections.Generic;

namespace EPCISEvent.Interfaces.DTOs
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
