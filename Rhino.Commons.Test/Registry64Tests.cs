#region license

// Copyright (c) 2005 - 2008 Ahmar Tareen (ahmar.tareen@gmail.com)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Ayende Rahien nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using MbUnit.Framework;

namespace Rhino.Commons.Test
{
    [TestFixture]
    public class Registry64Tests
    {
        [Test]
        public void CanRetrieveValueFromGivenRegistryKeyAndValueName()
        {
            //Given
            Registry64 localRegistry = Registry64.LocalMachine;

            //When
            string actual = localRegistry.GetValue(@"SOFTWARE\Microsoft\Windows\CurrentVersion", "MediaPathUnexpanded");

            //Then
            Assert.AreEqual(actual, @"%SystemRoot%\Media");
        }


        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void ShouldThrowExceptionWhenKeyOrValueNotFound()
        {
            Registry64.LocalMachine.GetValue(@"SOFTWARE\RUBBISH_KEY", "RUBBISH_VALUENAME");
        }
    }
}