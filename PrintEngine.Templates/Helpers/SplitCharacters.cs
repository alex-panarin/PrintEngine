using iText.IO.Font.Otf;
using iText.Layout.Splitting;

namespace PrintEngine.Templates.Helpers
{
	internal class SplitCharacters : DefaultSplitCharacters
	{
		public override bool IsSplitCharacter(GlyphLine text, int glyphPos)
		{
			var glyph = text.Get(glyphPos);
			if (!glyph.HasValidUnicode())
				return false;

			return glyph.GetUnicode() == ' '
				|| base.IsSplitCharacter(text, glyphPos);
		}
	}
}
