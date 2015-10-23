﻿namespace ManagedX.Input
{

	// https://msdn.microsoft.com/en-us/library/windows/desktop/ms648381%28v=vs.85%29.aspx


	/// <summary>Enumerates mouse cursor states.</summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "There is: Hidden." )]
	[System.Flags]
	public enum MouseCursorOptions : int
	{

		/// <summary>The cursor is hidden.</summary>
		Hidden = 0x00000000,

		/// <summary>The cursor is showing.</summary>
		Showing = 0x00000001,

		/// <summary>Windows 8: The cursor is suppressed.
		/// <para>This flag indicates that the system is not drawing the cursor because the user is providing input through touch or pen instead of the mouse.</para>
		/// </summary>
		Suppressed = 0x00000002

	}

}