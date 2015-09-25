using System;
using System.Windows;
using Topics.Radical.ComponentModel;
using Topics.Radical.Observers;
using Topics.Radical.Validation;

namespace Topics.Radical.Windows.Observers
{
    public class ObserversGroup : AbstractMonitor, IWeakEventListener
    {
        /// <summary>
        /// Creates a new ObserversGroup.
        /// </summary>
        /// <returns>The newly created ObserversGroup.</returns>
        public static ObserversGroup Create( Action onChangedHandler )
        {
            Ensure.That( onChangedHandler ).Named( "onChangedHandler" ).IsNotNull();

            return new ObserversGroup(onChangedHandler);
        }

        private Action onChangedHandler = () => { };

        private ObserversGroup(Action onChangedHandler)
        {
            this.onChangedHandler = onChangedHandler;
        }

        /// <summary>
        /// Adds a trigger monitor to the list of triggers.
        /// </summary>
        /// <param name="source">The source monitor.</param>
        /// <returns>
        /// An instance of the current ObserversGroup.
        /// </returns>
        public ObserversGroup AddMonitor( IMonitor source )
        {
            Ensure.That( source ).Named( "source" ).IsNotNull();
            MonitorChangedWeakEventManager.AddListener( source, this );

            return this;
        }

        /// <summary>
        /// Removes the given monitor from the list of triggers.
        /// </summary>
        /// <param name="source">The monitor to remove.</param>
        /// <returns>
        /// An instance of the current ObserversGroup.
        /// </returns>
        public ObserversGroup RemoveMonitor( IMonitor source )
        {
            Ensure.That( source ).Named( "source" ).IsNotNull();
            MonitorChangedWeakEventManager.RemoveListener( source, this );

            return this;
        }

        protected override void OnStopMonitoring( bool targetDisposed )
        {
            
        }

        protected override void OnChanged()
        {
            base.OnChanged();

            this.onChangedHandler();
        }

        /// <summary>
        /// Receives events from the centralized event manager.
        /// </summary>
        /// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager"/> calling this method.</param>
        /// <param name="sender">Object that originated the event.</param>
        /// <param name="e">Event data.</param>
        /// <returns>
        /// true if the listener handled the event. It is considered an error by the <see cref="T:System.Windows.WeakEventManager"/> handling in WPF to register a listener for an event that the listener does not handle. Regardless, the method should return false if it receives an event that it does not recognize or handle.
        /// </returns>
        bool IWeakEventListener.ReceiveWeakEvent( Type managerType, object sender, EventArgs e )
        {
            if( managerType == typeof( MonitorChangedWeakEventManager ) )
            {
                this.OnChanged();
            }
            else
            {
                // unrecognized event
                return false;
            }

            return true;
        }
    }
}