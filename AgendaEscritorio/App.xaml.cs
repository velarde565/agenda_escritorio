using AgendaEscritorio.simulated;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AgendaEscritorio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SimulatedServer? simulatedServer;  // Puede ser nulo inicialmente

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Inicializar el servidor simulado
            simulatedServer = new SimulatedServer();
            await simulatedServer.StartAsync();

            // Continuar con el flujo normal de la aplicación
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Si necesitas detener el servidor, implementa el método Stop en SimulatedServer.
            // simulatedServer?.Stop();  // Si decides añadirlo más adelante

            base.OnExit(e);
        }
    }


}
