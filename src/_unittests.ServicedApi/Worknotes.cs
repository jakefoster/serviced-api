using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using org.ncore.ServicedApi;
using org.ncore.ServicedApi.Container;

namespace _unittests.org.ncore.ServicedApi
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class Worknotes
    {
        public Worknotes()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Scratchpad()
        {
            // stuff goes here...
        }

        [TestMethod]
        public void DiIsh()
        {
            //Kernel.Registry.Add( "Samurai", typeof( Samurai ) );
            //Kernel.Registry.Add( typeof( Samurai ), typeof( Samurai ) );

            Kernel.Registry.Reset();

            Injector injector = new Injector( new InjectorRegistry(){
                new InjectorType( "Weapon", typeof(Katana) )
            } );

            /*
            Injector injector = new Injector( new InjectorRegistry(){
                new InjectorType( typeof(IWeapon), typeof(Sword) ),
                new InjectorType( "SpecialPower", typeof(SheerTerror) ),
                new InjectorType( "SecretPower", typeof(TemporaryBlindness) )
            } );
            */

            /*
            //dynamic myInstance = Instance.New( "Samurai", injector );
            dynamic myInstance = Instance.New( typeof( Samurai ), injector );
            //Debug.WriteLine( myInstance.UseSecretPower() );
            //Debug.WriteLine( myInstance.SpecialPower.Use() );
            string weapon_use = myInstance.Weapon.Use();
            Debug.WriteLine( weapon_use );
            */

            // OR

            Samurai mySamurai = Instance.New<Samurai>( injector );
            //Debug.WriteLine( mySamurai.UseSecretPower() );
            //Debug.WriteLine( mySamurai.SpecialPower.Use() );
            Debug.WriteLine( mySamurai.Weapon.Use() );
        }

        [TestMethod]
        public void Works_Expository()
        {
            Kernel.Registry.Add( new KernelType( typeof( Samurai ), typeof( Samurai ) ) );

            Injector injector = new Injector( new InjectorRegistry(){
                new InjectorType( typeof(IWeapon), typeof(Katana) ),
                new InjectorType( "Weapon", new Naginata(2) ),
                new InjectorType( "SpecialPower", typeof(SheerTerror) ),
                new InjectorType( "SecretPower", typeof(TemporaryBlindness) )
            } );

            Samurai mySamurai = Instance.New<Samurai>( injector );
            Assert.AreEqual( "Argh! My eyes!", mySamurai.UseSecretPower() );
            Assert.AreEqual( "Stop! You're scaring me!", mySamurai.SpecialPower.Use() );
            Assert.AreEqual( "Stab! Stab!", mySamurai.Weapon.Use() );
            Assert.AreEqual( "Slice!", mySamurai.AlternateWeapon.Use() );
        }

        [TestMethod]
        public void New_on_instance_with_named_dynamic_field()
        {
            Kernel.Registry.Reset();

            Injector injector = new Injector( new InjectorRegistry(){
                new InjectorType( "SecretPower", typeof(TemporaryBlindness) )
            } );

            Samurai mySamurai = Instance.New<Samurai>( injector );
            Assert.AreEqual( "Argh! My eyes!", mySamurai.UseSecretPower() );
        }

        [TestMethod]
        public void New_on_instance_with_unnamed_dynamic_property()
        {
            Kernel.Registry.Reset();

            Injector injector = new Injector( new InjectorRegistry(){
                new InjectorType( "SpecialPower", typeof(TemporaryBlindness) )
            } );

            Samurai mySamurai = Instance.New<Samurai>( injector );
            Assert.AreEqual( "Argh! My eyes!", mySamurai.SpecialPower.Use() );
        }
    }


    /* Injection play */
    public interface IWeapon
    {
        string Use();
    }

    public class Nagimaka : IWeapon
    {
        public string Use()
        {
            return "Big Slice!";
        }
    }

    public class Naginata : IWeapon
    {
        public int StabCount { get; set; }
        public string Use()
        {
            StringBuilder builder = new StringBuilder();
            for( int i = 1; i <= StabCount; i++ )
            {
                builder.Append( "Stab!" );
                if(i < StabCount)
                {
                    builder.Append( " " );
                }
            }
            return builder.ToString();
        }

        public Naginata(int stabCount)
        {
            StabCount = stabCount;
        }
    }

    public class Katana : IWeapon
    {
        public int StabCount { get; set; }
        public string Use()
        {
            StringBuilder builder = new StringBuilder();
            for( int i = 1; i <= StabCount; i++ )
            {
                builder.Append( "Slice!" );
                if( i < StabCount )
                {
                    builder.Append( " " );
                }
            }
            return builder.ToString();
        }

        public Katana()
        {
            StabCount = 1;
        }

        public Katana( int stabCount )
        {
            StabCount = stabCount;
        }
    }

    public class TemporaryBlindness
    {
        public string Use()
        {
            return "Argh! My eyes!";
        }
    }

    public class SheerTerror
    {
        public string Use()
        {
            return "Stop! You're scaring me!";
        }
    }

    public class Samurai
    {
        [Inject( "SecretPower" )]
        private dynamic _secretPower;
        [Inject]
        public dynamic SpecialPower { get; private set; }
        [Inject]
        public IWeapon Weapon { get; set; }
        [Inject( typeof( IWeapon ) )]
        public IWeapon AlternateWeapon { get; set; }

        public Samurai()
        {

        }

        public string UseSecretPower()
        {
            return _secretPower.Use();
        }
    }
}
