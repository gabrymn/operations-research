﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransportOptimizer.src.algo;
using TransportOptimizer.src.model;
using TransportOptimizer.src.utils;
using TransportOptimizer.src.view;

namespace TransportOptimizer.src.middleware
{
    internal class Method
    {
        private static int msTimeout = Const.MedMsTimeout;

        public static int MsTimeout
        {
            get => msTimeout;
            set { msTimeout = value < Const.MinMsTimeout ? Const.MinMsTimeout : value > Const.MaxMsTimeout ? Const.MaxMsTimeout : value; }
        }

        public void ResetTimeout() => msTimeout = Const.MedMsTimeout;

        public static bool Run(string? method, Table table, Main mainform)
        {
            List<SummaryData> data = new List<SummaryData>();
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = MsTimeout;
            bool status = false;

            var stopWatch = new System.Diagnostics.Stopwatch();

            timer.Tick += delegate
            {
                if (method == Const.Methods[0]) 
                    status = NorthwestCorner.Run(ref data, ref table);

                else if (method == Const.Methods[1]) 
                    status = LeastCost.Run(ref data, ref table);

                else if (method == Const.Methods[2])
                    status = VogelApprox.Run(ref data, ref table);

                else if (method == Const.Methods[3]) 
                    status = RussellApprox.Run(ref data, ref table);

                else
                    status = RussellApprox.Run(ref data, ref table);

                if (status == false)
                {
                    timer.Stop();
                    stopWatch.Stop();
                    var elapsed = stopWatch.ElapsedMilliseconds / 1000.00f;
                    new Summary(data.ToArray(), "nord_ovest", table, mainform, elapsed).ShowDialog();
                }
            };

            timer.Start();
            stopWatch.Start();

            return true;
        }
    }
}
