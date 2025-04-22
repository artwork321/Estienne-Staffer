EXTERNAL EnterInvestigationMode(int id)
EXTERNAL EnterConnectingClueMode(int id)

->BlackScreen

===BlackScreen===

COMMAND: #background:none

Port of London, 1945, 8:30 AM #speaker:Narrator #alias: //(sound of the typewriter getting pulled back)

->Immigration
    
===Immigration===
Papers Please #speaker:Inspector #background:hallway #music:Calm

“Huh?” #speaker:Estienne #flip:true #expression:normal #visibility:true:fading

Papers Please Sir. I am asking for your passport, and other legal documents you are bringing to Britain. #speaker:Inspector

Ah, yes, please give me a moment now.#speaker:Estienne

//sound of shuffling papers around

No rush sir, you seem to be our only person entering London through this method today.#speaker:Inspector

So it may seem… #speaker:Estienne #expression:panic

Is this your first time travelling to another country? #speaker:Inspector

... #speaker:Estienne //use this if you want to add pause at the moment (additional feature to-do but low priority)


Through ship, yes… why do you ask? #speaker:Estienne #expression:normal 

Aha! So I seem to be correct. You do seem to be confused when manoeuvring about this place, #speaker:Inspector

    so how is it so far? Where do you come from? #speaker:Inspector

"damn… why is this damn immigration officer so damn nosy… let just tell him what information he wants to know and lets just sod off… take it, where do I come from again?" #speaker:Estienne #expression:panic //thinks to self 


What should I give him? #speaker:Estienne

COMMAND: #speaker:Estienne #visibility:false:fading
~ EnterConnectingClueMode(0) 
COMMANDNONEXTLINE: // Exit the mode

Here’s my passport, and my documents sir. #speaker:Estienne #visibility:true:fading #expression:normal  #moveto:450:moving #music:Calm

ah yes, thank you #speaker:Inspector

... #speaker:Inspector

"Why is he looking at me like that?" #speaker:Estienne #expression:panic //thinks to self

am I perhaps missing something? #speaker:Estienne #moveto:-450:moving

no no, I just did not expect to see an east german on my doorsteps today, I'm a tad bit surprised #speaker:Inspector

I see… it's quite rare to see us after all. #speaker:Estienne

“so… what’s ya for staying?”#speaker:Inspector

seeking asylum sir #speaker:Estienne

how old are ye? #speaker:Inspector

26 #speaker:Estienne

And what is your name, laddie? #speaker:Inspector


"my name, my name… what’s my alias again?, I should check my passpor-" #speaker:Estienne //thinks to self

"Crap… it’s with him… what do I do?" #speaker:Estienne //thinks to self

COMMAND: #speaker:Estienne #visibility:false:fading
~ EnterConnectingClueMode(1)
COMMANDNONEXTLINE:

//Deduction 2: What's my name? I need two pieces of evidence to confirm my name

//Default: Select two clues/two pieces of solid evidence 

//Answer: Boat ticket name + Bank Check from Herman Estienne to Beret Estienne

"oh yes, I remember now" #speaker:Estienne #visibility:true:fading #music:Calm//thinks to self

Beret Estienne sir #speaker:Estienne

Estienne? #speaker:Inspector

Is there a problem sir? #speaker:Estienne #expression:panic

there is no problem sir.#speaker:Inspector

"wait, why is so polite all of a sudden?" #speaker:Estienne //thinks to self

All your information so far is accurate, and the documentation seems legitimate #speaker:Inspector

I see, is there any more procedures for me to do perhaps? #speaker:Estienne

There is none sir.#speaker:Inspector
 
For all I know I have no reason from stopping you entering Great Britain #speaker:Inspector

That’s great, thank you sir, and I’ll be on my merry way. #speaker:Estienne #expression:normal

//Estienne takes the documents back, and is about to walk off the screen

Oh yes, and one last word for you laddie #speaker:Inspector

yes? #speaker:Estienne

Consider changing that surname of yours #speaker:Inspector

uhmm… why is that the case? #speaker:Estienne

… #speaker:Inspector

uhhh #speaker:Estienne

you really don’t know? #speaker:Inspector

* I know... #speaker:Estienne #expression:panic
-> Know
* I believe I do not #speaker:Estienne #expression:panic
-> Dontknow

===Know===
I think I know... #speaker:Estienne #expression:panic

There is a man whose name is Estienne the Gasser... #speaker:Estienne #expression:normal

Yes, his name is Estienne the Gasser, who abused and utilised chemical warfare on the battlefield #speaker:Inspector

uhhh #speaker:Estienne #expression:panic

I know you are just an innocent lad, but that name gives the living freights to our veterans, #speaker:Inspector

->continue


===Dontknow===
During the war, there was a man who abused and utilised chemical warfare on the battlefield. #speaker:Inspector

His name, is Estienne the Gasser…#speaker:Inspector

uhhh #speaker:Estienne #expression:normal

I know you are just an innocent lad, but that name gives the living freights to our veterans, #speaker:Inspector
-> continue

===continue===
I see…#speaker:Estienne

oh… I’m terribly sorry for stopping you on your way… #speaker:Inspector

Oh, no it’s alright… #speaker:Estienne

and if that is the case, I bid you good luck Mr. Estienne. #speaker:Inspector


COMMAND: #background:none #speaker:Estienne #visibility:false:fading
Carpenter St. London #speaker:Narrator #alias:

I sat quietly in the middle of my coffee store… it had been quite a while since I had had a time of peace, #speaker:Narrator #alias:  #music:Calm2 #background:Town 

all the hard work over the years in working in a hospital as a chemist, but I found that to be boring. But ever since I came to this coffee store, I fell in love with it. #speaker:Narrator #alias:

COMMAND:
~ EnterInvestigationMode(0)
COMMANDNONEXTLINE:

Now that I own this store, and with my knowledge in organic chemistry, I’ve managed to improve the tastes of the coffees available, and with new tastes, I have gotten more customers as of lately.  #speaker:Estienne #flip:true #expression:normal #visibility:true:fading

Due to my coffee being relatively cheaper than most other stores, I have gained a bit of a customer following as of recent, such as the successful local real estate owner, Hugh Theravus, #speaker:Estienne

COMMANDNONEXTLINE: #speaker:NPC2 #expression:normal #moveto:450: #visibility:true:fading

The local military officer, Korey Wesson, and a surgical assistant, Grant Corvelli. Knowing them all well, they automatically go to their usual spots.  #speaker:Estienne #flip:true
COMMANDNONEXTLINE: #speaker:NPC3 #expression:normal #moveto:300: #visibility:true:fading

As I brewed another pot of coffee and prepare another plate of french toast, I hear my door open, and I see a relatively unknown man. #speaker:Estienne

Hello, welcome to Wellington’s, how may I help you today? #speaker:Estienne #flip:false


COMMANDNONEXTLINE: #speaker:NPC2 #visibility:false:fading
COMMANDNONEXTLINE: #speaker:NPC3 #visibility:false:fading
Ah, is Edmund still here? #speaker:NPC1 #alias:Unknown Man #moveto:450: #visibility:true:fading

I beg your pardon? #speaker:Estienne #expression:panic #flip:true

You know, the owner, Edmund Wellington. I used to be best mates in school with him you see, and the last I heard of him, he was opening a coffee shop of sorts. #speaker:NPC1 #alias:Unknown Man #expression:angry


Ah, I’m terribly sorry sir, but Mr. Wellington is no longer the owner of this store. I had bought this store from him a while back when his business was about go bankrupt  #speaker:Estienne

Damn… well, that was an interesting run was it not? #speaker:NPC1 #alias:Unknown Man #expression:normal

I believe so. His coffee was the best, and the location very peaceful and convenient #speaker:Estienne #expression:normal

It’s a shame he’s gone bankrupt like this. #speaker:Estienne

Well yes it is… #alias:Unknown Man #expression:normal

"I look at the man, he seemed to be displaying a face of nostalgia." #speaker:Estienne

COMMAND: #background:none
To be continue #speaker:Narrator
->DONE




