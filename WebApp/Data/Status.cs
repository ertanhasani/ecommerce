﻿namespace WebApp.Data;

public partial class Status
{
    public Status()
    {
        Order = new HashSet<Order>();
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<Order> Order { get; set; }
}