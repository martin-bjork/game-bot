﻿using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace GameBot.Tests {

    public class DummyTest {

        [Test]
        public void DummyTestSimplePasses() {
            int actualValue = 3 + 7;
            int expectedValue = 10;
            Assert.That(actualValue, Is.EqualTo(expectedValue), "Addition should simply work");
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator DummyTestWithEnumeratorPasses() {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}
