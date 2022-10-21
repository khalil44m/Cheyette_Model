# Cheyette_Model
Forward Rate Model

This repo contains a restitued work from a previous experience of implementing an Interest Forward Rate pricing model.

Cheyette model belongs to the class of continuous no-arbitrage Term Structure models (known as [Heath-Jarrow-Merton Framework][1]). In the HJM framework, the drift of no-arbitrage evolution of the instantaneous forward rate ican be expressed as a function of its volatilities and correlation among them. In general, The markovian property is sadly compromiseed when looking at the SDE.  Oren Cheyette deals with the non-markovian limitation (path dependence complexity) by imposing a forward rate volatility term structure.

The SDE is then represented by a set of state variables that describe the whole diffusion of the short rate.

The Solution also includes some acceleration methods to overcome PDE methods limitations. Indeed, PDE methods typically do not work for these high-dimensional models. The method introduced here is called '[Particle Method][2]', which is a Monte Carlo method where the simulated asset paths interact with each other so as to ensure that a given market smile (or several of them) is fitted. The SDE then not only diffuses the underlying (inst. forward rate), but also its distribution (cf. [McKean-Vlasov Equation][3]).

In the context of Swaption pricing, vanilla options, on one hand, can be calibrated with a purely local volatility structure, achieved without the Particle Method. On the other hand, exotic options pricing includes an effect of VolVol and its calibration requires up to introduce a mixed local/stochastic structure.

The Runner/Run.Console Folder is the main entry point of the Solution. Calibration and model parameters include few hyperparameters for the user to tune them.

[1]:https://en.wikipedia.org/wiki/Heath–Jarrow–Morton_framework
[2]:https://deliverypdf.ssrn.com/delivery.php?ID=875082005085007109072005031100104092018052053087053016092066096124082079025113105026038106063111031098097096025107000110065064029018023080043017108091068119127124088008042111088092067091121120118081108120125127022007007123096064125080094084084092093&EXT=pdf&INDEX=TRUE
[3]:https://www.iam.uni-bonn.de/fileadmin/user_upload/ywip2014/Talks/Santiago.pdf
