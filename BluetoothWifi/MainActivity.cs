using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Bluetooth;
using Android.Content;
using Android.Support.V4.Content;
using Android.Content.PM;
using Android.Support.V4.App;
using System;
using Android.Net.Wifi;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace BluetoothWifi
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
        int requestPermissions;
        string bluetoothPermission = Android.Manifest.Permission.Bluetooth;
        string bluetoothadminPermission = Android.Manifest.Permission.BluetoothAdmin;
        string locationPermission = Android.Manifest.Permission.AccessCoarseLocation;
        string locationfinePermission = Android.Manifest.Permission.AccessFineLocation;
        string wifiPermission = Android.Manifest.Permission.AccessWifiState;
        string changewifiPermission = Android.Manifest.Permission.ChangeWifiState;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            if (!(ContextCompat.CheckSelfPermission(this, locationPermission) == (int)Permission.Granted))
            {
                ActivityCompat.RequestPermissions(this, new String[] { locationPermission, locationfinePermission, bluetoothPermission, bluetoothadminPermission, wifiPermission, changewifiPermission }, requestPermissions);
            }

            bluetoothAdapter.Enable();

            bluetoothAdapter.StartDiscovery();

            BluetoothDeviceReceiver bluetoothDeviceReceiver = new BluetoothDeviceReceiver();
            RegisterReceiver(bluetoothDeviceReceiver, new IntentFilter(BluetoothDevice.ActionFound));

            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.WifiService);
            wifiManager.StartScan();

            WifiReceiver wifiReceiver = new WifiReceiver();
            RegisterReceiver(wifiReceiver, new IntentFilter(WifiManager.ScanResultsAvailableAction));
            /*var networks = wifiManager.ConfiguredNetworks;
            string wifi = "";

            for (int i = 0; i < networks.Count; i++)
            {
                wifi = wifi + networks[i].Ssid + "\r\n";
                DisplayTextWifi(wifi);
            }*/


            /*string networkSSID = "WIFI-GUEST";
            string networkPass = "";

            WifiConfiguration wifiConfig = new WifiConfiguration();
            wifiConfig.Ssid = string.Format("\"{0}\"", networkSSID);
            wifiConfig.PreSharedKey = string.Format("\"{0}\"", networkPass);

            int netId = wifiManager.AddNetwork(wifiConfig);
            wifiManager.Disconnect();
            wifiManager.EnableNetwork(netId, true);
            wifiManager.Reconnect();*/

        }
        public class BluetoothDeviceReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                var mainActivity = (MainActivity)context;
                string action = intent.Action;

                if (action == BluetoothDevice.ActionFound)
                {
                    // Get device
                    BluetoothDevice newDevice = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                    string message = newDevice.ToString();
                    string bluetooth = "";
                    bluetooth = bluetooth + message + "\r\n";
                    mainActivity.DisplayTextBluetooth(bluetooth);
                }
            }
        }
        public class WifiReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                var mainActivity = (MainActivity)context;
                string action = intent.Action;

                if (action == WifiManager.ScanResultsAvailableAction)
                {
                    WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.WifiService);
                    IList<ScanResult> scanResults = wifiManager.ScanResults;
                }
            }
        }
        public void DisplayTextBluetooth(string text)
        {
            FindViewById<TextView>(Resource.Id.bluetooth).Text = "Bluetooth networks: \r\n" + text;
        }
        public void DisplayTextWifi(string text)
        {
            FindViewById<TextView>(Resource.Id.txtScanResults).Text = "Wifi networks: \r\n" + text;
        }

    }
}