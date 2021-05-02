using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatrolePumpSceduler;
using Vehicles;
using PatroleStation;

namespace PetrolePumpTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void VehicleTankTest()
        {
            Vehicle v = new Vehicle();
            double allreadyFilled = 0;

            if(v.Type == Vehicle_Type.Cars)
            {
                allreadyFilled = 12.5;
            }
            else if(v.Type == Vehicle_Type.HGV)
            {
                allreadyFilled = 37.5;
            }
            else
            {
                allreadyFilled = 20;
            }

            Assert.IsTrue(allreadyFilled >= v.Tank && v.Tank >= 0, "Fuel must be 1/4 of total capacity and greater then 0.");
        }

        [TestMethod]
        public void QueueTest()
        {
            VehicleContainer vc = new VehicleContainer();
            Thread.Sleep(2500);
            Assert.AreEqual(vc.getVehicles()[0], vc.getVehicleToServed(), "Vehicle Should be first one to be served!");
        }

        [TestMethod]
        public void ExpiryTest()
        {
            VehicleContainer vc = new VehicleContainer();
            Thread.Sleep(2500+3000);

            Assert.IsTrue((vc.TotalExpired > 0), "Vehicle Should be expired after 3000ms max!");
        }

        [TestMethod]
        public void PumpLockTest()
        {
            Vehicle v = new Vehicle();
            Pump p = new Pump("p1");
            Assert.IsTrue(p.isAvailable , "Pump should be not lock before serving vehicle!");
            try
            {
                p.Serve(v);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception due to Update function!");
            }
            
            Assert.IsFalse(p.isAvailable , "Pump should be lock after serving vehicle!");
        }

        [TestMethod]
        public void StationAdvanceQueueTest()
        {
            Vehicle v = new Vehicle();
            Pump p1 = new Pump("p1");
            Pump p2 = new Pump("p2");
            Pump p3 = new Pump("p2");

            Assert.IsTrue(p2.isAvailable && p3.isAvailable, "Pump 2 and 3 should be not lock before serving vehicle to pump 1!");
            try
            {
                p1.Serve(v,p2,p3);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception due to Update function!");
            }

            Assert.IsFalse(p2.isAvailable && p3.isAvailable, "Pump 2 and 3 should be lock after serving vehicle to pump 1!");
        }

        [TestMethod]
        public void PumpUnlockTest()
        {

            Vehicle v = new Vehicle();
            Pump p = new Pump("p1");
            try
            {
                p.Serve(v);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception due to Update function!");
            }

            Assert.IsFalse(p.isAvailable, "Pump should be lock after serving vehicle!");
            Thread.Sleep(20000);
            Assert.IsTrue(p.isAvailable, "Pump should be unlock after serving vehicle!");
        }

        [TestMethod]
        public void Dispensedtest()
        {

            Vehicle v = new Vehicle();
            double tankCapacity = v.Tank;
            Pump p = new Pump("p1");
            Assert.AreEqual(p.Dispensed, 0, "Pump's dispensed should be 0 liters in begining!");
            try
            {
                p.Serve(v);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception due to Update function!");
            }

            Thread.Sleep(20000);
            double dispensedLiters = v.Tank - tankCapacity;
            Assert.AreEqual(p.Dispensed, dispensedLiters,0.5, "Pump's dispensed should be equal to actual dispensed!");
        }


    }
}
