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
        public void Works_Expository()
        {
            Kernel.Registry.Reset();
            Kernel.Registry.Add( new KernelType( typeof( Fighter ), typeof( Samurai ) ) );
            
            Injector injector = new Injector( new InjectorRegistry{
                { typeof(IThrowableWeapon), typeof(ThrowingStar) },
                { "Weapon", new Naginata(2) },
                { "AlternateWeapon", new Katana(){SliceCount = 3} },
                { "_secretPower", typeof(SheerTerror) },
                { "SpecialPower", typeof(TemporaryBlindness) }
            } );

            // NOTE: Notice above that setting "_secretPower" doesn't pop. This is exactly
            //  what we want.  It allows us to not know *everything* about the actual
            //  type that gets created until runtime.  We know it's of type Fighter but
            //  we don't know if it's a Ninja or a Samurai. If it IS a Ninja, then the
            //  _secretPower field will be set and we can later conditionally call
            //  a Ninja specific method to reveal the secret power (see below).  -JF

            Fighter myFighter = Instance.New<Fighter>( injector );
            Assert.AreEqual( typeof( Samurai ), myFighter.GetType() );
            Assert.AreEqual( "Whizzz, Thud!", myFighter.ThrowableWeapon.Throw() );
            Assert.AreEqual( "Stab! Stab!", myFighter.Weapon.Use() );
            Assert.AreEqual( "Slice! Slice! Slice!", myFighter.AlternateWeapon.Use() );
            if( myFighter is Ninja )
            {
                // NOTE: If you've got a secret power, use it!
                Assert.AreEqual( "Stop! You're scaring me!", ((Ninja)myFighter).UseSecretPower() );
            }
            Assert.AreEqual( "Hey! Who turned out the lights!", myFighter.SpecialPower.Use() );
        }

        [TestMethod]
        public void Works_dynamic_expository()
        {
            Kernel.Registry.Reset();
            //Kernel.Registry.Add( new KernelType( "Fighter", typeof( Samurai ) ) );
            Kernel.Registry.Add( new KernelType( typeof( Fighter ), typeof( Samurai ) ) );

            Injector injector = new Injector( new InjectorRegistry{
                { typeof(IThrowableWeapon), typeof(ThrowingStar) },
                { "Weapon", new Naginata(2) },
                { "AlternateWeapon", new Katana(){SliceCount = 3} },
                { "_secretPower", typeof(SheerTerror) },
                { "SpecialPower", typeof(TemporaryBlindness) }
            } );

            //dynamic myInstance = Instance.New( "Fighter", injector );
            dynamic myFighter = Instance.New( typeof( Fighter ), injector );
            Assert.AreEqual( typeof( Samurai ), myFighter.GetType() );
            Assert.AreEqual( "Whizzz, Thud!", myFighter.ThrowableWeapon.Throw() );
            Assert.AreEqual( "Stab! Stab!", myFighter.Weapon.Use() );
            Assert.AreEqual( "Slice! Slice! Slice!", myFighter.AlternateWeapon.Use() );
            if( myFighter is Ninja )
            {
                // NOTE: If you've got a secret power, use it!
                Assert.AreEqual( "Stop! You're scaring me!", ( (Ninja)myFighter ).UseSecretPower() );
            }
            Assert.AreEqual( "Hey! Who turned out the lights!", myFighter.SpecialPower.Use() );
        }

        [TestMethod]
        public void Works_Ninja_Expository()
        {
            Kernel.Registry.Reset();
            Kernel.Registry.Add( new KernelType( typeof( Fighter ), typeof( Ninja ) ) );

            Injector injector = new Injector( new InjectorRegistry{
                { typeof(IThrowableWeapon), typeof(GlassDust) },
                { "Weapon", new Naginata(2) },
                { "AlternateWeapon", new Katana(){SliceCount = 3} },
                { "_secretPower", typeof(SheerTerror) },
                { "SpecialPower", typeof(TemporaryBlindness) }
            } );

            Fighter myFighter = Instance.New<Fighter>( injector );
            Assert.AreEqual( typeof( Ninja ), myFighter.GetType() );
            Assert.AreEqual( "Puff... Gah! My eyes!!", myFighter.ThrowableWeapon.Throw() );
            Assert.AreEqual( "Stab! Stab!", myFighter.Weapon.Use() );
            Assert.AreEqual( "Slice! Slice! Slice!", myFighter.AlternateWeapon.Use() );
            Assert.AreEqual( "Stop! You're scaring me!", ((Ninja)myFighter).UseSecretPower() );
            Assert.AreEqual( "Hey! Who turned out the lights!", myFighter.SpecialPower.Use() );
        }

        
        [TestMethod]
        public void New_on_instance_with_dynamic_field()
        {
            Kernel.Registry.Reset();

            Injector injector = new Injector( new InjectorRegistry{
                { "_secretPower", typeof(TemporaryBlindness) }
            } );

            Ninja myNinja = Instance.New<Ninja>( injector );
            Assert.AreEqual( "Hey! Who turned out the lights!", myNinja.UseSecretPower() );
        }

        [TestMethod]
        public void New_on_instance_with_dynamic_property()
        {
            Kernel.Registry.Reset();

            Injector injector = new Injector( new InjectorRegistry{
                { "SpecialPower", typeof(TemporaryBlindness) }
            } );

            Samurai mySamurai = Instance.New<Samurai>( injector );
            Assert.AreEqual( "Hey! Who turned out the lights!", mySamurai.SpecialPower.Use() );
        }
    }


    /* Injection play */
    public interface IBladedWeapon
    {
        string Use();
    }

    public class Nagimaka : IBladedWeapon
    {
        public string Use()
        {
            return "Big Slice!";
        }
    }

    public class Naginata : IBladedWeapon
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

        public Naginata()
        {
        }

        public Naginata(int stabCount)
        {
            StabCount = stabCount;
        }
    }

    public class Katana : IBladedWeapon
    {
        public int SliceCount { get; set; }
        public string Use()
        {
            StringBuilder builder = new StringBuilder();
            for( int i = 1; i <= SliceCount; i++ )
            {
                builder.Append( "Slice!" );
                if( i < SliceCount )
                {
                    builder.Append( " " );
                }
            }
            return builder.ToString();
        }

        public Katana()
        {
            SliceCount = 1;
        }

        public Katana( int stabCount )
        {
            SliceCount = stabCount;
        }
    }

    public interface IThrowableWeapon
    {
        string Throw();
    }

    public class ThrowingStar : IThrowableWeapon
    { 
        public string Throw()
        {
            return "Whizzz, Thud!";
        }
    }

    public class GlassDust : IThrowableWeapon
    {
        public string Throw()
        {
            return "Puff... Gah! My eyes!!";
        }
    }

    public class TemporaryBlindness
    {
        public string Use()
        {
            return "Hey! Who turned out the lights!";
        }
    }

    public class SheerTerror
    {
        public string Use()
        {
            return "Stop! You're scaring me!";
        }
    }

    #pragma warning disable 649 // Field '_unittests.org.ncore.ServicedApi.Ninja._secretPower' is never assigned to, and will always have its default value null
    public class Ninja : Fighter
    {
        private dynamic _secretPower;

        public string UseSecretPower()
        {
            return _secretPower.Use();
        }
    }
    #pragma warning restore 649

    public class Samurai : Fighter
    {
        /*
        private dynamic _secretPower;

        public dynamic SpecialPower { get; private set; }
        public IBladedWeapon Weapon { get; set; }
        public IBladedWeapon AlternateWeapon { get; set; }
        public IThrowableWeapon ThrowableWeapon { get; set; }

        public Samurai()
        {

        }

        public string UseSecretPower()
        {
            return _secretPower.Use();
        }
         */
    }

    public class Fighter
    {

        public dynamic SpecialPower { get; private set; }
        public IBladedWeapon Weapon { get; set; }
        public IBladedWeapon AlternateWeapon { get; set; }
        public IThrowableWeapon ThrowableWeapon { get; set; }

        public Fighter()
        {

        }
    }
}
