﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;


namespace ManagedX.Input
{
	using Raw;


	/// <summary>A mouse.</summary>
	public sealed class Mouse : RawInputDevice<MouseState, MouseButton>
	{

		private const int MaxSupportedMice = 4; // FIXME - actually only the primary mouse is properly supported... and this should be set to 2

		/// <summary>Defines the maximum number of supported mouse buttons: 5.</summary>
		public const int MaxSupportedButtonCount = 5;


		/// <summary>Enumerates mouse buttons, using their <see cref="VirtualKeyCode"/>.</summary>
		private enum ButtonVirtualKeyCode : int
		{

			/// <summary>No button.</summary>
			None = VirtualKeyCode.None,

			/// <summary>The left mouse button.</summary>
			Left = VirtualKeyCode.MouseLeft,

			/// <summary>The right mouse button.</summary>
			Right = VirtualKeyCode.MouseRight,

			/// <summary>The middle mouse button.</summary>
			Middle = VirtualKeyCode.MouseMiddle,

			/// <summary>The extended button 1.</summary>
			X1 = VirtualKeyCode.MouseX1,

			/// <summary>The extended button 2.</summary>
			X2 = VirtualKeyCode.MouseX2

		}


		[Win32.Native( "WinUser.h" )]
		[SuppressUnmanagedCodeSecurity]
		private static class SafeNativeMethods
		{

			private const string LibraryName = "User32.dll";


			/// <summary>Retrieves information about the global cursor.</summary>
			/// <param name="cursorInfo">A valid <see cref="CursorInfo"/> structure that receives the information.</param>
			/// <returns>If the function succeeds, the return value is true. If the function fails, the return value is false.
			/// <para>To get extended error information, call GetLastError.</para>
			/// </returns>
			[DllImport( LibraryName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true )]
			[return: MarshalAs( UnmanagedType.Bool )]
			internal static extern bool GetCursorInfo(
				[In, Out] ref CursorInfo cursorInfo
			);
			// https://msdn.microsoft.com/en-us/library/windows/desktop/ms648389%28v=vs.85%29.aspx


			/// <summary>Moves the cursor to the specified screen coordinates.
			/// If the new coordinates are not within the screen rectangle set by the most recent ClipCursor function call, the system automatically adjusts the coordinates so that the cursor stays within the rectangle.</summary>
			/// <param name="x">The new x-coordinate of the cursor, in screen coordinates.</param>
			/// <param name="y">The new y-coordinate of the cursor, in screen coordinates.</param>
			/// <returns>Returns true if successful or false otherwise.</returns>
			/// <remarks>The cursor is a shared resource. A window should move the cursor only when the cursor is in the window's client area.
			/// <para>The calling process must have WINSTA_WRITEATTRIBUTES access to the window station.</para>
			/// <para>
			/// The input desktop must be the current desktop when you call SetCursorPos.
			/// Call OpenInputDesktop to determine whether the current desktop is the input desktop.
			/// If it is not, call SetThreadDesktop with the HDESK returned by OpenInputDesktop to switch to that desktop.
			/// </para>
			/// </remarks>
			[DllImport( LibraryName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = true )]
			[return: MarshalAs( UnmanagedType.Bool )]
			internal static extern bool SetCursorPos(
				[In] int x,
				[In] int y
			);
			// https://msdn.microsoft.com/en-us/library/windows/desktop/ms648394%28v=vs.85%29.aspx


			/// <summary>Displays or hides the cursor.</summary>
			/// <param name="show">If true, the display count is incremented by one; otherwise, the display count is decremented by one.</param>
			/// <returns>The return value specifies the new display counter.</returns>
			/// <remarks>
			/// <para>Windows 8: Call <see cref="GetCursorInfo"/> to determine the cursor cursorState.</para>
			/// This function sets an internal display counter that determines whether the cursor should be displayed.
			/// The cursor is displayed only if the display count is greater than or equal to 0.
			/// If a mouse is installed, the initial display count is 0.
			/// If no mouse is installed, the display count is –1.
			/// </remarks>
			[DllImport( LibraryName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
			internal static extern int ShowCursor(
				[In, MarshalAs( UnmanagedType.Bool )] bool show
			);
			// https://msdn.microsoft.com/en-us/library/windows/desktop/ms648396%28v=vs.85%29.aspx


			/// <summary>Determines whether a key is up or down at the time the function is called, and whether the key was pressed after a previous call to <see cref="GetAsyncKeyState"/>.</summary>
			/// <param name="key">The virtual-key code.</param>
			/// <returns>If the function succeeds, the return value specifies whether the key was pressed since the last call to GetAsyncKeyState, and whether the key is currently up or down. If the most significant bit is set, the key is down, and if the least significant bit is set, the key was pressed after the previous call to GetAsyncKeyState. However, you should not rely on this last behavior; for more information, see the Remarks.
			/// The return value is zero for the following cases:
			/// <para>The current desktop is not the active desktop</para>
			/// <para>The foreground thread belongs to another process and the desktop does not allow the hook or the journal record.</para>
			/// </returns>
			/// <remarks>
			/// <para>The GetAsyncKeyState function works with mouse buttons.
			/// However, it checks on the state of the physical mouse buttons, not on the logical mouse buttons that the physical buttons are mapped to.
			/// For example, the call GetAsyncKeyState(VK_LBUTTON) always returns the state of the left physical mouse button, regardless of whether it is mapped to the left or right logical mouse button.
			/// You can determine the system's current mapping of physical mouse buttons to logical mouse buttons by calling GetSystemMetrics(SM_SWAPBUTTON) which returns true if the mouse buttons have been swapped.
			/// </para>
			/// <para>Although the least significant bit of the return value indicates whether the key has been pressed since the last query, due to the pre-emptive multitasking nature of Windows, another application can call GetAsyncKeyState and receive the "recently pressed" bit instead of your application.
			/// The behavior of the least significant bit of the return value is retained strictly for compatibility with 16-bit Windows applications (which are non-preemptive) and should not be relied upon.
			/// </para>
			/// <para>You can use the virtual-key code constants VK_SHIFT, VK_CONTROL, and VK_MENU as values for the vKey parameter.
			/// This gives the state of the SHIFT, CTRL, or ALT keys without distinguishing between left and right.
			/// </para>
			/// </remarks>
			[DllImport( LibraryName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
			internal static extern short GetAsyncKeyState(
				[In] ButtonVirtualKeyCode key
			);
			// https://msdn.microsoft.com/en-us/library/windows/desktop/ms646293%28v=vs.85%29.aspx

		}


		#region Static

		private static ButtonVirtualKeyCode ToVirtualKeyCode( int buttonIndex )
		{
			if( buttonIndex == (int)MouseButton.Left )
				return ButtonVirtualKeyCode.Left;

			if( buttonIndex == (int)MouseButton.Right )
				return ButtonVirtualKeyCode.Right;

			if( buttonIndex == (int)MouseButton.Middle )
				return ButtonVirtualKeyCode.Middle;

			if( buttonIndex == (int)MouseButton.X1 )
				return ButtonVirtualKeyCode.X1;

			if( buttonIndex == (int)MouseButton.X2 )
				return ButtonVirtualKeyCode.X2;

			return ButtonVirtualKeyCode.None;
		}


		private static MouseCursorOptions cursorState;


		/// <summary>Gets or sets a value indicating the state of the mouse cursor.
		/// <para>Note: <see cref="MouseCursorOptions.Suppressed"/> is handled as <see cref="MouseCursorOptions.Hidden"/>.</para>
		/// </summary>
		public static MouseCursorOptions CursorState
		{
			get { return cursorState; }
			set
			{
				if( value.HasFlag( MouseCursorOptions.Suppressed ) )
					value = MouseCursorOptions.Hidden;

				cursorState = value;

				if( cursorState == MouseCursorOptions.Showing )
					while( SafeNativeMethods.ShowCursor( true ) < 0 ) ;
				else
					while( SafeNativeMethods.ShowCursor( false ) >= 0 ) ;
			}
		}


		/// <summary>Sets the mouse cursor position.
		/// <para>A window should move the cursor only when the cursor is in the window's client area.</para>
		/// </summary>
		/// <param name="position">The new mouse cursor position, relative to the desktop.</param>
		/// <exception cref="Win32Exception"/>
		public static void SetCursorPosition( Point position )
		{
			if( !SafeNativeMethods.SetCursorPos( position.X, position.Y ) )
				throw new Win32Exception( "Failed to set mouse cursor location.", NativeMethods.GetExceptionForLastWin32Error() );
		}

		#endregion Static



		private MouseState state;
		internal Point motionDelta;
		internal int wheelDelta;
		private int wheelValue;
		private MouseDeviceInfo info;



		internal Mouse( GameControllerIndex controllerIndex, ref RawInputDeviceDescriptor descriptor )
			: base( (int)controllerIndex, ref descriptor )
		{
			var zero = TimeSpan.Zero;
			this.Reset( ref zero );
		}



		/// <summary>Resets the state and information about this <see cref="Mouse"/>.</summary>
		/// <param name="time">The time elapsed since the start of the application.</param>
		protected sealed override void Reset( ref TimeSpan time )
		{
			motionDelta = Point.Zero;
			wheelValue = wheelDelta = 0;

			base.Reset( ref time );

			var deviceInfo = base.Info.MouseInfo;
			if( deviceInfo != null && deviceInfo.HasValue )
				info = deviceInfo.Value;
			else
				info = MouseDeviceInfo.Empty;
		}


		/// <summary>Returns the mouse state.
		/// <para>This method is called by Reset and Update.</para>
		/// </summary>
		/// <returns>Returns the mouse state.</returns>
		/// <exception cref="Win32Exception"/>
		protected sealed override MouseState GetState()
		{
			const short Mask = -32768;

			var cursorInfo = CursorInfo.Default;
			if( !SafeNativeMethods.GetCursorInfo( ref cursorInfo ) )
			{
				var lastException = NativeMethods.GetExceptionForLastWin32Error();
				if( lastException.HResult == (int)Win32.ErrorCode.NotConnected )
				{
					base.IsDisconnected = true;
					wheelValue = wheelDelta = 0;
					motionDelta = Point.Zero;
					return MouseState.Empty;
				}
				throw new Win32Exception( "Failed to retrieve mouse cursor position.", lastException );
			}
			cursorState = cursorInfo.State;

			var buttons = 0;
			for( var b = 0; b < MaxSupportedButtonCount; b++ )
				if( ( SafeNativeMethods.GetAsyncKeyState( ToVirtualKeyCode( b ) ) & Mask ) == Mask )
					buttons |= 1 << b;

			wheelDelta /= 120;

			//state = new MouseState( ref cursorInfo.ScreenPosition, ref motion, wheelDelta, (MouseButtons)buttons );
			state.position = cursorInfo.ScreenPosition;
			state.motion = motionDelta;
			state.wheel = wheelDelta;
			state.buttons = (MouseButtons)buttons;

			motionDelta.X = motionDelta.Y = 0;
			wheelValue += wheelDelta;
			wheelDelta = 0;

			return state;
		}


		/// <summary>Gets a value indicating whether a button is pressed in the current state and released in the previous state.</summary>
		/// <param name="button">A mouse button.</param>
		/// <returns>Returns true if the specified <paramref name="button"/> is pressed in the current state and released in the previous state, otherwise returns false.</returns>
		public sealed override bool HasJustBeenPressed( MouseButton button )
		{
			if( base.IsDisconnected )
				return false;

			return base.CurrentState[ button ] && !base.PreviousState[ button ];
		}


		/// <summary>Gets a value indicating whether a button is released in the current state and pressed in the previous state.</summary>
		/// <param name="button">A mouse button.</param>
		/// <returns>Returns true if the specified <paramref name="button"/> is released in the current state and pressed in the previous state, otherwise returns false.</returns>
		public sealed override bool HasJustBeenReleased( MouseButton button )
		{
			if( base.IsDisconnected )
				return false;

			return !base.CurrentState[ button ] && base.PreviousState[ button ];
		}


		/// <summary>Gets or sets the cumulated wheel value.</summary>
		public int WheelValue
		{
			get { return wheelValue; }
			set { wheelValue = value; }
		}


		#region Device info

		/// <summary>Gets the identifier of this <see cref="Mouse"/> device.</summary>
		public int Id { get { return info.Id; } }


		/// <summary>Gets the number of buttons of this <see cref="Mouse"/>.</summary>
		public int ButtonCount { get { return info.ButtonCount; } }


		/// <summary>Gets a value indicating whether this <see cref="Mouse"/> device has an horizontal scroll wheel.</summary>
		public bool HasHorizontalWheel { get { return info.HasHorizontalWheel; } }


		/// <summary>Gets the number of data points per second; this information may not be applicable to every <see cref="Mouse"/> devices.</summary>
		public int SampleRate { get { return info.SampleRate; } }

		#endregion Device info


		/// <summary>Gets the index of this <see cref="Mouse"/>.</summary>
		new public GameControllerIndex Index { get { return (GameControllerIndex)base.Index; } }

	}

}