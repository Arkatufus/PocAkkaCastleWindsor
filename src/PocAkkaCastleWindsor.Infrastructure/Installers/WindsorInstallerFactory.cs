using Castle.MicroKernel.Registration;
using Castle.Windsor.Installer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAkkaCastleWindsor.Infrastructure.Installers
{
    public class WindsorInstallerFactory : InstallerFactory
    {
        public override IEnumerable<Type> Select(IEnumerable<Type> installerTypes)
        {
            var windsorInfrastructureInstaller = installerTypes.FirstOrDefault(
                it => it == typeof(WindsorInfrastructureInstaller));

            var retVal = new List<Type>();
            retVal.Add(windsorInfrastructureInstaller);
            retVal.AddRange(installerTypes
                .Where(it =>
                    typeof(IWindsorInstaller).IsAssignableFrom(it) &&
                    !retVal.Contains(it)
                    ));

            return retVal;
        }
    }
}
