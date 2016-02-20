﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;


namespace ManagedX.Input
{
	using Raw;

	
	/// <summary>A keyboard.</summary>
	public sealed class Keyboard : RawInputDevice<KeyboardState, Key>
	{

		private const int MaxSupportedKeyboards = 4;


		[SuppressUnmanagedCodeSecurity]
		private static class SafeNativeMethods
		{

			private const string LibraryName = "User32.dll";
			// WinUser.h


			/// <summary>Copies the status of the 256 virtual keys to the specified buffer.</summary>
			/// <param name="state">Receives a 256-byte array containing the status data for each virtual key.</param>
			/// <returns>Returns true on success, otherwise returns false.</returns>
			[DllImport( LibraryName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true )]
			[return: MarshalAs( UnmanagedType.Bool )]
			internal static extern bool GetKeyboardState(
				[Out, MarshalAs( UnmanagedType.LPArray, SizeConst = 256 )] byte[] state
			);
			// https://msdn.microsoft.com/en-us/library/windows/desktop/ms646299%28v=vs.85%29.aspx
			//BOOL GetKeyboardState(
			//	_Out_writes_(256) PBYTE lpKeyState
			//);

		}


		#region Static

		private static readonly List<Keyboard> keyboardList = new List<Keyboard>( 1 );


		private static void OnKeyboardDisconnected( object sender, EventArgs e )
		{
			var keyboard = (Keyboard)sender;
			
			keyboard.Disconnected -= OnKeyboardDisconnected;

			keyboardList.Remove( keyboard );
		}


		private static void Initialize()
		{
			var allDevices = NativeMethods.GetRawInputDeviceList();
			var index = 0;
			for( var d = 0; d < allDevices.Length; d++ )
			{
				var descriptor = allDevices[ d ];
				if( descriptor.DeviceType == InputDeviceType.Keyboard )
				{
					var keyboard = new Keyboard( (GameControllerIndex)index, ref descriptor );
					if( !keyboard.IsDisconnected )
					{
						keyboardList.Add( keyboard );
						keyboard.Disconnected += OnKeyboardDisconnected;

						if( ++index == MaxSupportedKeyboards )
							break;
					}
				}
			}
		}

		
		/// <summary>Gets the default keyboard.</summary>
		public static Keyboard Default
		{
			get
			{
				if( keyboardList.Count == 0 )
					Initialize();

				if( keyboardList.Count == 0 )
					return null;

				return keyboardList[ 0 ];
			}
		}


		/// <summary>Gets a read-only collection containing all known (up to 4) keyboards.</summary>
		public static ReadOnlyCollection<Keyboard> All { get { return new ReadOnlyCollection<Keyboard>( keyboardList ); } }


		///// <summary>Causes the target window to receive raw keyboard input messages.
		///// <para>Important: that window must then override its WndProc method to call <see cref="WndProc"/> prior to its base method.</para>
		///// </summary>
		///// <param name="targetWindow">The target window.</param>
		///// <param name="options">One or more <see cref="RawInputDeviceRegistrationOptions"/>.</param>
		//public static void Register( System.Windows.Forms.IWin32Window targetWindow, RawInputDeviceRegistrationOptions options )
		//{
		//	var device = RawInputDevice.KeyboardDefault;
		//	device.targetWindowHandle = ( targetWindow == null ) ? IntPtr.Zero : targetWindow.Handle;
		//	device.flags = options;

		//	if( keyboards.Count == 0 )
		//		Initialize();

		//	try
		//	{
		//		NativeMethods.RegisterRawInputDevices( device );
		//	}
		//	catch( Exception )
		//	{
		//		throw;
		//	}
		//}


		///// <summary>Processes window messages to ensure the mouse motion and wheel state are up-to-date.</summary>
		///// <param name="message">A Windows message.</param>
		//[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Required by implementation." )]
		//public static void WndProc( ref System.Windows.Forms.Message message )
		//{
		//	if( message.Msg == 254 ) // WindowMessage.InputDeviceChange
		//	{
		//		var wParam = message.WParam.ToInt32();
		//		// Device arrival (wParam == 1) or removal (wParam == 2)
		//		// TODO - mark the device as disconnected on removal, otherwise initialize a new RawInputDevice.
		//	}
		//	else if( message.Msg == 255 ) // WindowMessage.Input
		//	{
		//		RawInput rawInput;
		//		NativeMethods.GetRawInputData( message.LParam, out rawInput );
		//		if( rawInput.DeviceType == InputDeviceType.Keyboard )
		//		{
		//			Keyboard targetKeyboard;
		//			if( !keyboards.TryGetValue( rawInput.DeviceHandle, out targetKeyboard ) )
		//				return;
		//		}
		//	}
		//}

		#endregion Static


		
		/// <summary>Constructor.</summary>
		/// <param name="controllerIndex">The keyboard index.</param>
		/// <param name="descriptor">The keyboard descriptor.</param>
		private Keyboard( GameControllerIndex controllerIndex, ref RawInputDeviceDescriptor descriptor )
			: base( controllerIndex, ref descriptor )
		{
			var zero = TimeSpan.Zero;
			this.Reset( ref zero );
		}

		

		/// <summary>Returns a value indicating whether a key is pressed in the current state and released in the previous state.</summary>
		/// <param name="button">A keyboard key.</param>
		/// <returns>Returns true if the key specified by <paramref name="button"/> is pressed in the current state and released in the previous state, otherwise returns false.</returns>
		public sealed override bool HasJustBeenPressed( Key button )
		{
			if( base.IsDisconnected )
				return false;

			return base.CurrentState[ button ] && !base.PreviousState[ button ];
		}


		/// <summary>Returns a value indicating whether a key is released in the current state and pressed in the previous state.</summary>
		/// <param name="button">A keyboard key.</param>
		/// <returns>Returns true if the key specified by <paramref name="button"/> is released in the current state and pressed in the previous state, otherwise returns false.</returns>
		public sealed override bool HasJustBeenReleased( Key button )
		{
			if( base.IsDisconnected )
				return false;

			return !base.CurrentState[ button ] && base.PreviousState[ button ];
		}


		/// <summary>Retrieves the keyboard state and returns it.
		/// <para>This method is called by Reset and Update.</para>
		/// </summary>
		/// <returns>Returns a <see cref="KeyboardState"/> structure representing the current state of the keyboard.</returns>
		/// <exception cref="Win32Exception"/>
		protected sealed override KeyboardState GetState()
		{
			KeyboardState state;
			state.Data = new byte[ 256 ];

			if( !( base.IsDisconnected = !SafeNativeMethods.GetKeyboardState( state.Data ) ) )
				return state;

			var lastException = GetLastWin32Exception();
			if( lastException.HResult == (int)ErrorCode.NotConnected )
				return KeyboardState.Empty;

			throw new Win32Exception( "Failed to retrieve keyboard state.", lastException );
		}


		/// <summary>Gets information about the keyboard device.</summary>
		public KeyboardDeviceInfo DeviceInfo { get { return base.Info.KeyboardInfo.Value; } }

	}

}