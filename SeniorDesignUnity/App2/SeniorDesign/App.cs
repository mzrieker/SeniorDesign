using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using UnityPlayer;
using Windows.UI.Xaml;
using Windows.System;

namespace SeniorDesign
{
    private int _pinEnable = 25;
    private int _pinRightF = 12;
    private int _pinRightB = 16;
    private int _pinLeftF = 21;
    private int _pinLeftB = 20;

    private GpioController _controller;
    private GpioPin _motorEnable;
    private GpioPin _motorControlRightF;
    private GpioPin _motorControlRightB;
    private GpioPin _motorControlLeftF;
    private GpioPin _motorControlLeftB;

    class App : IFrameworkView, IFrameworkViewSource
    {
        private WinRTBridge.WinRTBridge m_Bridge;
        private AppCallbacks m_AppCallbacks;

        public App()
        {
            
            SetupOrientation();
            m_AppCallbacks = new AppCallbacks();

            // Allow clients of this class to append their own callbacks.
            AddAppCallbacks(m_AppCallbacks);

            if (Window.Current.CoreWindow.GetKeyState(VirtualKey.R).HasFlag(CoreVirtualKeyStates.Down))
            {
                System.Threading.Tasks.Task.Delay(300).Wait();
                
            }
            else if (Window.Current.CoreWindow.GetKeyState(VirtualKey.O).HasFlag(CoreVirtualKeyStates.Down))
            {
                System.Threading.Tasks.Task.Delay(300).Wait();
                _forwardMotor();
            }
            else if (Window.Current.CoreWindow.GetKeyState(VirtualKey.P).HasFlag(CoreVirtualKeyStates.Down))
            {
                System.Threading.Tasks.Task.Delay(300).Wait();
                _turnOffIgnition();
            }
        }

        public virtual void Initialize(CoreApplicationView applicationView)
        {
            applicationView.Activated += ApplicationView_Activated;
            CoreApplication.Suspending += CoreApplication_Suspending;

            // Setup scripting bridge
            m_Bridge = new WinRTBridge.WinRTBridge();
            m_AppCallbacks.SetBridge(m_Bridge);

            m_AppCallbacks.SetCoreApplicationViewEvents(applicationView);
        }

        /// <summary>
        /// This is where apps can hook up any additional setup they need to do before Unity intializes.
        /// </summary>
        /// <param name="appCallbacks"></param>
        virtual protected void AddAppCallbacks(AppCallbacks appCallbacks)
        {
        }

        private void CoreApplication_Suspending(object sender, SuspendingEventArgs e)
        {
        }

        private void ApplicationView_Activated(CoreApplicationView sender, IActivatedEventArgs args)
        {
            CoreWindow.GetForCurrentThread().Activate();
        }

        public void SetWindow(CoreWindow coreWindow)
        {
            ApplicationView.GetForCurrentView().SuppressSystemOverlays = true;

            m_AppCallbacks.SetCoreWindowEvents(coreWindow);
            m_AppCallbacks.InitializeD3DWindow();
        }

        public void Load(string entryPoint)
        {
        }

        public void Run()
        {
            m_AppCallbacks.Run();
        }

        public void Uninitialize()
        {
        }

        [MTAThread]
        static void Main(string[] args)
        {
            var app = new App();
            CoreApplication.Run(app);
        }

        public IFrameworkView CreateView()
        {
            return this;
        }

        private void SetupOrientation()
        {
            Unity.UnityGenerated.SetupDisplay();
        }
    }
}
