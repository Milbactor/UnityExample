using UnityEngine;
using System.Collections;

public class GeekTypeWriter : TypewriterEffect {

	protected bool done;
	
	public delegate void OnElapsed( GeekTypeWriter writer);
	public event OnElapsed onElapsed;
	
	// Use this for initialization
	void Start () {
	
	}
	
	void Update ()
	{
		if (mReset)
		{
			mOffset = 0;
			mReset = false;
			mLabel = GetComponent<UILabel>();
			mText = mLabel.processedText;
		}
		
		if (mOffset < mText.Length && mNextChar <= RealTime.time)
		{
			charsPerSecond = Mathf.Max(1, charsPerSecond);
			
			// Periods and end-of-line characters should pause for a longer time.
			float delay = 1f / charsPerSecond;
			char c = mText[mOffset];
					
			if (c == '.')
			{
				if (mOffset + 2 < mText.Length && mText[mOffset + 1] == '.' && mText[mOffset + 2] == '.')
				{
					delay += delayOnPeriod * 3f;
					mOffset += 2;
				}
				else delay += delayOnPeriod;
			}
			else if (c == '!' || c == '?')
			{
				delay += delayOnPeriod;
			}
			else if (c == '\n') delay += delayOnNewLine;
			
			// Automatically skip all symbols
			NGUIText.ParseSymbol(mText, ref mOffset);
			
			mNextChar = RealTime.time + delay;
			mLabel.text = mText.Substring(0, ++mOffset);
			
			//print ("charsper second : " + charsPerSecond + " | delay : " + delay );
			
			// If a scroll view was specified, update its position
			if (scrollView != null) scrollView.UpdatePosition();
		}
		else if( done == false && mOffset > 0 )
		{
			//print ("done");
			done = true;
			
			if(onElapsed != null )onElapsed( this );
		}
	}
	
	public void reset()
	{
		done = false;
		mReset = true;
	}
}
