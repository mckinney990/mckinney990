using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

namespace MulliganWallet
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class StartActivity : AppCompatActivity
    {
        private Button btnGoToLogin, btnGoToRegistration;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.start);

            btnGoToLogin = FindViewById<Button>(Resource.Id.btnGotoLoginFromStart);
            btnGoToRegistration = FindViewById<Button>(Resource.Id.btnGotoRegisterFromStart);

            btnGoToLogin.Click += BtnGoToLogin_Click;
            btnGoToRegistration.Click += BtnGoToRegistration_Click;
        }

        private void BtnGoToLogin_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoginActivity));
            this.StartActivity(intent);
        }

        private void BtnGoToRegistration_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegistrationActivity));
            this.StartActivity(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}