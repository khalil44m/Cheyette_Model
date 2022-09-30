# Cheyette_Model
Forward Rate Model

This repo contains a restitued work from a previous experience of implementing a Interest Forward Rate pricing model.

Cheyette model belongs to the class of continuous no-arbitrage Term Structure models (known as Heath-Jarrow-Merton Framework). O. Cheyette dealt with the non-markovian propery (path dependence) by imposing a forward rate volatility term structure.

The Solution presented includes some acceleration methods to overcome PDE methods limitations. The latters typically do not work for these high-dimensional models. The method introduced here is called '[Particle Method]'[1], which is a Monte Carlo method where the simulated asset paths interact with each other so as to ensure that a given market smile (or several of them) is fitted.  

[1]:https://deliverypdf.ssrn.com/delivery.php?ID=875082005085007109072005031100104092018052053087053016092066096124082079025113105026038106063111031098097096025107000110065064029018023080043017108091068119127124088008042111088092067091121120118081108120125127022007007123096064125080094084084092093&EXT=pdf&INDEX=TRUE
