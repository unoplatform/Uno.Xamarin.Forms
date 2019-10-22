﻿using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

using WContentPresenter = Windows.UI.Xaml.Controls.ContentPresenter;

namespace Xamarin.Forms.Platform.UWP
{
	public partial class FormsButton : Windows.UI.Xaml.Controls.Button
	{
		public static readonly DependencyProperty BorderRadiusProperty = DependencyProperty.Register(nameof(BorderRadius), typeof(int), typeof(FormsButton),
			new PropertyMetadata(default(int), OnBorderRadiusChanged));

		public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(FormsButton),
			new PropertyMetadata(default(Brush), OnBackgroundColorChanged));

		WContentPresenter _contentPresenter;

#if __IOS__
		public new Brush BackgroundColor
#else
		public Brush BackgroundColor
#endif
		{
			get
			{
				return (Brush)GetValue(BackgroundColorProperty);
			}
			set
			{
				SetValue(BackgroundColorProperty, value);
			}
		}

		public int BorderRadius
		{
			get
			{
				return (int)GetValue(BorderRadiusProperty);
			}
			set
			{
				SetValue(BorderRadiusProperty, value);
			}
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_contentPresenter = GetTemplateChild("ContentPresenter") as WContentPresenter;

			UpdateBackgroundColor();
			UpdateBorderRadius();
		}

		static void OnBackgroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((FormsButton)d).UpdateBackgroundColor();
		}

		static void OnBorderRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((FormsButton)d).UpdateBorderRadius();
		}

		void UpdateBackgroundColor()
		{
			if (BackgroundColor == null)
				BackgroundColor = Background;

			if (_contentPresenter != null)
				_contentPresenter.Background = BackgroundColor;
			Background = Color.Transparent.ToBrush();
		}

		void UpdateBorderRadius()
		{
			if (_contentPresenter != null)
			{
				var radius = BorderRadius == -1 ? 0 : BorderRadius;
				_contentPresenter.CornerRadius = new Windows.UI.Xaml.CornerRadius(radius);
			}
		}

		public void UpdateCharacterSpacing(int characterSpacing)
		{
			CharacterSpacing = characterSpacing;

			if (_contentPresenter != null)
				_contentPresenter.CharacterSpacing = CharacterSpacing;

			if(Content is TextBlock tb)
			{
				tb.CharacterSpacing = CharacterSpacing;
			}

			if (Content is StackPanel sp)
			{
				foreach (var item in sp.Children)
				{
					if (item is TextBlock textBlock)
					{
						textBlock.CharacterSpacing = CharacterSpacing;
					}
				}
			}

		}
	}
}