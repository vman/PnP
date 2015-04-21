﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using OfficeDevPnP.Core.Utilities;
using System.Xml.Linq;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;

namespace OfficeDevPnP.PowerShell.Commands.Branding
{
    [Cmdlet("Apply", "SPOProvisioningTemplate")]
    [CmdletHelp("Applies a provisioning template to a web", Category = "Branding")]
    public class ApplyProvisioningTemplate : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, HelpMessage = "Path to the xml file containing the provisioning template.")]
        public string Path;

        
        protected override void ExecuteCmdlet()
        {
            if (!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }

            FileInfo fileInfo = new FileInfo(Path);

            XMLTemplateProvider provider =
                new XMLFileSystemTemplateProvider(fileInfo.DirectoryName, "");

            var provisioningTemplate = provider.GetTemplate(fileInfo.Name);

            if (provisioningTemplate != null)
            {
                var fileinfo = new FileInfo(Path);
                var fileSystemConnector = new FileSystemConnector(fileinfo.DirectoryName, "");
                provisioningTemplate.Connector = fileSystemConnector;
                SelectedWeb.ApplyProvisioningTemplate(provisioningTemplate);
            }
        }
    }
}