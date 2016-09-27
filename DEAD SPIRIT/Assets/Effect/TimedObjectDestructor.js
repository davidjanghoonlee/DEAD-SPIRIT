var timeOut = 1.0;
var detachChildren = false;

function Awake ()
{
	Invoke ("DestroyNow", timeOut/2);
}

function DestroyNow ()
{
	if (detachChildren) {
		transform.DetachChildren ();
	}
	DestroyObject (gameObject);
}