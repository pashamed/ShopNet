﻿namespace ShopNet.DAL.Entities.OrderAggregate
{
    public class Address
    {
        public Address()
        {
        }

        public Address(string firstName, string lastName, string street, string city, string postalCode, string country)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            PostalCode = postalCode;
            Country = country;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}