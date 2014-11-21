using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using System;
using Spectator.Core.Model.Push;
using Spectator.Core.Model.Inject;
using Microsoft.Practices.ServiceLocation;

namespace Spectator.WP8.ScheduledTaskAgent
{
    public class ScheduledAgent : Microsoft.Phone.Scheduler.ScheduledTaskAgent
    {
        public ScheduledAgent()
        {
            var locator = new SpectatorServiceLocator(new WPInjectModule());
            ServiceLocator.SetLocatorProvider(() => locator);
        }

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected async override void OnInvoke(ScheduledTask task)
        {
            await new PushModel().HandleNewSyncMessage();

            // If debugging is enabled, launch the agent again in one minute.
#if DEBUG
            ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(60));
#endif

            // Call NotifyComplete to let the system know the agent is done working.
            NotifyComplete();
        }

        public static void StartPeriodicAgent()
        {
            var periodicTaskName = "PeriodicAgent";

            // Obtain a reference to the period task, if one exists
            var periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;

            // If the task already exists and background agents are enabled for the
            // application, you must remove the task and then add it again to update 
            // the schedule
            if (periodicTask != null)
            {
                try
                {
                    ScheduledActionService.Remove(periodicTaskName);
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            periodicTask = new PeriodicTask(periodicTaskName) { Description = "This demonstrates a periodic task." };

            // Place the call to Add in a try block in case the user has disabled agents.
            try
            {
                ScheduledActionService.Add(periodicTask);

                // If debugging is enabled, use LaunchForTest to launch the agent in one minute.
#if (DEBUG)
                ScheduledActionService.LaunchForTest(periodicTaskName, TimeSpan.FromSeconds(5));
#endif
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
    }
}