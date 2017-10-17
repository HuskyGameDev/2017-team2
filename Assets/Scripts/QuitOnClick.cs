<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnClick : MonoBehaviour {

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnClick : MonoBehaviour {

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
>>>>>>> b308f26e25419288c1f908ae159334fe73a95b34
