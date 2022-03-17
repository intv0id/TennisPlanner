namespace TennisPlanner.Core.Clients
{
    using System;

    interface ITransportClient
    {
        DateTime GetTransportationTime(DateTime arrivalTime, string adress);
    }
}
