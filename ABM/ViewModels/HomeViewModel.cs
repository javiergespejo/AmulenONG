﻿using ABM.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ABM.ViewModels
{
    public class HomeViewModel
    {        
        public List<byte[]> SliderImages { get; set; }
        public string WelcomeText { get; set; }
        public List<Proyect> Projects { get; set; }
    }
}