using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.Model;

namespace WebsiteScrapper.CountryApp
{
    public class ISO3166CountryCodeModel: ScrapObject<object>
    {
        public ScrapString WhatisISO3166 { get; set; }
        public ScrapString ISO3166CountryCode { get; set; }
        public ISO31661Alpha2Model ISO31661Alpha2 { get; set; }
    }
}
